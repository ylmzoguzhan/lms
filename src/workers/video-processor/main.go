package main

import (
	"context"
	"encoding/json"
	"fmt"
	"log"
	"os"
	"os/exec"
	"path/filepath"

	"github.com/google/uuid"
	"github.com/minio/minio-go/v7"
	"github.com/minio/minio-go/v7/pkg/credentials"
	amqp "github.com/rabbitmq/amqp091-go"
)

/* ================================
   CONTRACTS
================================ */

type VideoUploadedEvent struct {
	VideoId     string `json:"videoId"`
	BlobPath    string `json:"blobPath"`
	ContentType string `json:"contentType"`
}

type VideoProcessedEvent struct {
	VideoId string `json:"videoId"`
	HlsPath string `json:"hlsPath"`
	Success bool   `json:"success"`
}

/* ================================
   MASS TRANSIT ENVELOPE
================================ */

type MTEnvelope[T any] struct {
	MessageId   string   `json:"messageId"`
	MessageType []string `json:"messageType"`
	Message     T        `json:"message"`
}

/* ================================
   GLOBALS
================================ */

var (
	minioClient *minio.Client
	rabbitConn  *amqp.Connection
	rabbitCh    *amqp.Channel
)

func main() {
	initMinio()
	initRabbit()
	defer rabbitConn.Close()
	defer rabbitCh.Close()

	q, err := rabbitCh.QueueDeclare(
		"media-video-processor",
		true, false, false, false, nil,
	)
	failOnError(err, "Queue declare failed")

	err = rabbitCh.QueueBind(
		q.Name,
		"",
		"Shared.Contracts.Media:VideoUploadedIntegrationEvent",
		false,
		nil,
	)
	failOnError(err, "Queue bind failed")

	rabbitCh.Qos(1, 0, false)

	msgs, err := rabbitCh.Consume(
		q.Name,
		"",
		false,
		false,
		false,
		false,
		nil,
	)
	failOnError(err, "Consume failed")

	log.Println("✅ Worker hazır. Mesaj bekleniyor...")

	for d := range msgs {

		log.Printf("RAW JSON:\n%s\n", string(d.Body))

		var envelope MTEnvelope[VideoUploadedEvent]

		if err := json.Unmarshal(d.Body, &envelope); err != nil {
			log.Printf("❌ JSON Error: %v", err)
			d.Ack(false)
			continue
		}

		event := envelope.Message

		log.Printf("📥 İşleme Başlandı: %s", event.VideoId)

		err := handleVideoProcessing(event)

		if err != nil {
			log.Printf("❌ Processing Failed: %v", err)
			sendCallback(event.VideoId, "", false)
			d.Nack(false, false)
		} else {
			log.Printf("✅ Processing Completed: %s", event.VideoId)
			sendCallback(
				event.VideoId,
				fmt.Sprintf("processed/%s/index.m3u8", event.VideoId),
				true,
			)
			d.Ack(false)
		}
	}
}

func handleVideoProcessing(event VideoUploadedEvent) error {

	tempDir := fmt.Sprintf("./temp_%s", event.VideoId)
	inputPath := tempDir + "_original.mp4"
	outputDir := filepath.Join(tempDir, "hls")

	os.MkdirAll(outputDir, os.ModePerm)
	defer os.RemoveAll(tempDir)
	defer os.Remove(inputPath)

	err := minioClient.FGetObject(
		context.Background(),
		"lms-media",
		event.BlobPath,
		inputPath,
		minio.GetObjectOptions{},
	)
	if err != nil {
		return fmt.Errorf("download error: %w", err)
	}

	fileInfo, err := os.Stat(inputPath)
	if err != nil {
		return fmt.Errorf("file stat error: %w", err)
	}

	if fileInfo.Size() == 0 {
		return fmt.Errorf("downloaded file is empty")
	}

	log.Println("🎬 FFmpeg transcode başlıyor...")

	cmd := exec.Command(
		"ffmpeg", "-i", inputPath,
		"-profile:v", "baseline",
		"-level", "3.0",
		"-s", "1280x720",
		"-start_number", "0",
		"-hls_time", "10",
		"-hls_list_size", "0",
		"-f", "hls",
		filepath.Join(outputDir, "index.m3u8"),
	)

	if out, err := cmd.CombinedOutput(); err != nil {
		return fmt.Errorf("ffmpeg error: %v, output: %s", err, string(out))
	}

	return uploadFolderToMinio(
		outputDir,
		fmt.Sprintf("processed/%s", event.VideoId),
	)
}

func uploadFolderToMinio(localDir, remotePrefix string) error {
	files, _ := os.ReadDir(localDir)

	for _, f := range files {
		localPath := filepath.Join(localDir, f.Name())
		remotePath := fmt.Sprintf("%s/%s", remotePrefix, f.Name())

		_, err := minioClient.FPutObject(
			context.Background(),
			"lms-media",
			remotePath,
			localPath,
			minio.PutObjectOptions{},
		)
		if err != nil {
			return err
		}
	}
	return nil
}

func sendCallback(videoId, hlsPath string, success bool) {

	callbackEvent := VideoProcessedEvent{
		VideoId: videoId,
		HlsPath: hlsPath,
		Success: success,
	}

	envelope := MTEnvelope[VideoProcessedEvent]{
		MessageId: uuid.New().String(),
		MessageType: []string{
			"urn:message:Shared.Contracts.Media:VideoProcessedIntegrationEvent",
		},
		Message: callbackEvent,
	}

	body, _ := json.Marshal(envelope)

	err := rabbitCh.PublishWithContext(
		context.Background(),
		"Shared.Contracts.Media:VideoProcessedIntegrationEvent",
		"",
		false,
		false,
		amqp.Publishing{
			ContentType: "application/vnd.masstransit+json",
			Body:        body,
		},
	)

	if err != nil {
		log.Printf("⚠️ Callback gönderilemedi: %v", err)
	}
}

func initMinio() {
	var err error

	minioClient, err = minio.New("localhost:9000", &minio.Options{
		Creds:  credentials.NewStaticV4("admin", "admin1234", ""),
		Secure: false,
	})

	failOnError(err, "Minio init failed")
}

func initRabbit() {
	var err error

	rabbitConn, err = amqp.Dial("amqp://guest:guest@localhost:5672/")
	failOnError(err, "Rabbit connection failed")

	rabbitCh, err = rabbitConn.Channel()
	failOnError(err, "Rabbit channel failed")
}

func failOnError(err error, msg string) {
	if err != nil {
		log.Fatalf("%s: %s", msg, err)
	}
}

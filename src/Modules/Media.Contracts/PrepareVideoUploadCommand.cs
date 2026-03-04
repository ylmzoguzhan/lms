using Shared.Abstractions.Request;

namespace Media.Contracts;

public record UploadVideoCommand(string FileName, string ContentType) : ICommand<UploadVideoResponse>;

public record UploadVideoResponse(Guid VideoId, string UploadUrl);

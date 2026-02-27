using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Media.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMedia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                schema: "media",
                table: "Videos",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                schema: "media",
                table: "Videos",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "media",
                table: "Videos",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAtUtc",
                schema: "media",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                schema: "media",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "media",
                table: "Videos");
        }
    }
}

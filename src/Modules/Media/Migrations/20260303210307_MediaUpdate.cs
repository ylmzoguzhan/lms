using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Media.Migrations
{
    /// <inheritdoc />
    public partial class MediaUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                schema: "media",
                table: "Videos",
                newName: "FileName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FileName",
                schema: "media",
                table: "Videos",
                newName: "Title");
        }
    }
}

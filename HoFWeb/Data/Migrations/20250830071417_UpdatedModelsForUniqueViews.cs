using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoFWeb.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedModelsForUniqueViews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UniqueViewsCount",
                table: "Screenshots",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UniqueViews",
                table: "ScreenshotDataPoints",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UniqueViewsCount",
                table: "Screenshots");

            migrationBuilder.DropColumn(
                name: "UniqueViews",
                table: "ScreenshotDataPoints");
        }
    }
}

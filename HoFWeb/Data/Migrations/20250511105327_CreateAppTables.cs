using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoFWeb.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateAppTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Creators",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatorNameSlug = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatorNameLocale = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatorNameLatinized = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatorNameTranslated = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Creators", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScreenshotDataPoints",
                columns: table => new
                {
                    ScreenshotScreenshotDataPointId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Favorites = table.Column<int>(type: "int", nullable: false),
                    Views = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScreenshotDataPoints", x => x.ScreenshotScreenshotDataPointId);
                });

            migrationBuilder.CreateTable(
                name: "Screenshots",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    IsReported = table.Column<bool>(type: "bit", nullable: false),
                    FavoritesCount = table.Column<int>(type: "int", nullable: false),
                    FavoritesPerDay = table.Column<double>(type: "float", nullable: false),
                    FavoritingPercentage = table.Column<double>(type: "float", nullable: false),
                    ViewsCount = table.Column<int>(type: "int", nullable: false),
                    ViewsPerDay = table.Column<double>(type: "float", nullable: false),
                    CityName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CityNameLocale = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CityNameLatinized = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CityNameTranslated = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CityMilestone = table.Column<int>(type: "int", nullable: false),
                    CityPopulation = table.Column<int>(type: "int", nullable: false),
                    ImageUrlThumbnail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrlFHD = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl4K = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAtFormatted = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAtFormattedDistance = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Favorited = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Screenshots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Screenshots_Creators_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Creators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Screenshots_CreatorId",
                table: "Screenshots",
                column: "CreatorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScreenshotDataPoints");

            migrationBuilder.DropTable(
                name: "Screenshots");

            migrationBuilder.DropTable(
                name: "Creators");
        }
    }
}

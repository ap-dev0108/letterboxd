using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.Migrations
{
    /// <inheritdoc />
    public partial class UpdatingModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Films_MovieRatedmovieId",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_MovieRatedmovieId",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "MovieRatedmovieId",
                table: "Ratings");

            migrationBuilder.CreateTable(
                name: "FilmRatings",
                columns: table => new
                {
                    FilmsmovieId = table.Column<Guid>(type: "uuid", nullable: false),
                    MovieRatingsRatingId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilmRatings", x => new { x.FilmsmovieId, x.MovieRatingsRatingId });
                    table.ForeignKey(
                        name: "FK_FilmRatings_Films_FilmsmovieId",
                        column: x => x.FilmsmovieId,
                        principalTable: "Films",
                        principalColumn: "movieId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FilmRatings_Ratings_MovieRatingsRatingId",
                        column: x => x.MovieRatingsRatingId,
                        principalTable: "Ratings",
                        principalColumn: "RatingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FilmRatings_MovieRatingsRatingId",
                table: "FilmRatings",
                column: "MovieRatingsRatingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FilmRatings");

            migrationBuilder.AddColumn<Guid>(
                name: "MovieRatedmovieId",
                table: "Ratings",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_MovieRatedmovieId",
                table: "Ratings",
                column: "MovieRatedmovieId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Films_MovieRatedmovieId",
                table: "Ratings",
                column: "MovieRatedmovieId",
                principalTable: "Films",
                principalColumn: "movieId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

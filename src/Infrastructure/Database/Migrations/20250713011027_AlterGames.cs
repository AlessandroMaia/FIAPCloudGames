using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AlterGames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "gender",
                schema: "fcg",
                table: "games");

            migrationBuilder.AddColumn<string>(
                name: "genre",
                schema: "fcg",
                table: "games",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Game_Genre",
                schema: "fcg",
                table: "games",
                sql: "Genre IN ('Action', 'Adventure', 'RPG', 'Strategy', 'FPS', 'Sports', 'Puzzle', 'Horror', 'Simulation')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Game_Genre",
                schema: "fcg",
                table: "games");

            migrationBuilder.DropColumn(
                name: "genre",
                schema: "fcg",
                table: "games");

            migrationBuilder.AddColumn<string>(
                name: "gender",
                schema: "fcg",
                table: "games",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}

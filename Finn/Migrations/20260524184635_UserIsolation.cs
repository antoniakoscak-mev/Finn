using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Finn.Migrations
{
    /// <inheritdoc />
    public partial class UserIsolation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Troskovi",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Prihodi",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Budzeti",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Troskovi");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Prihodi");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Budzeti");
        }
    }
}

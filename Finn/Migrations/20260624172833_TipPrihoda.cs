using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Finn.Migrations
{
    /// <inheritdoc />
    public partial class TipPrihoda : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TipPrihoda",
                table: "Prihodi",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TipPrihoda",
                table: "Prihodi");
        }
    }
}

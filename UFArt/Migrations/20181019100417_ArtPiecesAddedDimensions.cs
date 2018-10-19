using Microsoft.EntityFrameworkCore.Migrations;

namespace UFArt.Migrations
{
    public partial class ArtPiecesAddedDimensions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Dimensions",
                table: "ArtPieces",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dimensions",
                table: "ArtPieces");
        }
    }
}

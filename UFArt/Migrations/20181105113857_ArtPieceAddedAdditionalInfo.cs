using Microsoft.EntityFrameworkCore.Migrations;

namespace UFArt.Migrations
{
    public partial class ArtPieceAddedAdditionalInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "News",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Header",
                table: "News",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdditionalInfo",
                table: "ArtPieces",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalInfo",
                table: "ArtPieces");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "News",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Header",
                table: "News",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}

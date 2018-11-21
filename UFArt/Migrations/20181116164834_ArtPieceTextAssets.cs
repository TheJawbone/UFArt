using Microsoft.EntityFrameworkCore.Migrations;

namespace UFArt.Migrations
{
    public partial class ArtPieceTextAssets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalInfo",
                table: "ArtPieces");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "ArtPieces");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "ArtPieces");

            migrationBuilder.AddColumn<int>(
                name: "AdditionalInfoId",
                table: "ArtPieces",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DescriptionId",
                table: "ArtPieces",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NameId",
                table: "ArtPieces",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArtPieces_AdditionalInfoId",
                table: "ArtPieces",
                column: "AdditionalInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtPieces_DescriptionId",
                table: "ArtPieces",
                column: "DescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtPieces_NameId",
                table: "ArtPieces",
                column: "NameId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArtPieces_TextAssets_AdditionalInfoId",
                table: "ArtPieces",
                column: "AdditionalInfoId",
                principalTable: "TextAssets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ArtPieces_TextAssets_DescriptionId",
                table: "ArtPieces",
                column: "DescriptionId",
                principalTable: "TextAssets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ArtPieces_TextAssets_NameId",
                table: "ArtPieces",
                column: "NameId",
                principalTable: "TextAssets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArtPieces_TextAssets_AdditionalInfoId",
                table: "ArtPieces");

            migrationBuilder.DropForeignKey(
                name: "FK_ArtPieces_TextAssets_DescriptionId",
                table: "ArtPieces");

            migrationBuilder.DropForeignKey(
                name: "FK_ArtPieces_TextAssets_NameId",
                table: "ArtPieces");

            migrationBuilder.DropIndex(
                name: "IX_ArtPieces_AdditionalInfoId",
                table: "ArtPieces");

            migrationBuilder.DropIndex(
                name: "IX_ArtPieces_DescriptionId",
                table: "ArtPieces");

            migrationBuilder.DropIndex(
                name: "IX_ArtPieces_NameId",
                table: "ArtPieces");

            migrationBuilder.DropColumn(
                name: "AdditionalInfoId",
                table: "ArtPieces");

            migrationBuilder.DropColumn(
                name: "DescriptionId",
                table: "ArtPieces");

            migrationBuilder.DropColumn(
                name: "NameId",
                table: "ArtPieces");

            migrationBuilder.AddColumn<string>(
                name: "AdditionalInfo",
                table: "ArtPieces",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ArtPieces",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ArtPieces",
                nullable: true);
        }
    }
}

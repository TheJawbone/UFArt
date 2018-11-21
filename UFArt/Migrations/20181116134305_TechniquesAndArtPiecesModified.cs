using Microsoft.EntityFrameworkCore.Migrations;

namespace UFArt.Migrations
{
    public partial class TechniquesAndArtPiecesModified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodeName",
                table: "Techniques");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Techniques");

            migrationBuilder.DropColumn(
                name: "Technique",
                table: "ArtPieces");

            migrationBuilder.AddColumn<int>(
                name: "NameId",
                table: "Techniques",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TechniqueID",
                table: "ArtPieces",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Techniques_NameId",
                table: "Techniques",
                column: "NameId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtPieces_TechniqueID",
                table: "ArtPieces",
                column: "TechniqueID");

            migrationBuilder.AddForeignKey(
                name: "FK_ArtPieces_Techniques_TechniqueID",
                table: "ArtPieces",
                column: "TechniqueID",
                principalTable: "Techniques",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Techniques_TextAssets_NameId",
                table: "Techniques",
                column: "NameId",
                principalTable: "TextAssets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArtPieces_Techniques_TechniqueID",
                table: "ArtPieces");

            migrationBuilder.DropForeignKey(
                name: "FK_Techniques_TextAssets_NameId",
                table: "Techniques");

            migrationBuilder.DropIndex(
                name: "IX_Techniques_NameId",
                table: "Techniques");

            migrationBuilder.DropIndex(
                name: "IX_ArtPieces_TechniqueID",
                table: "ArtPieces");

            migrationBuilder.DropColumn(
                name: "NameId",
                table: "Techniques");

            migrationBuilder.DropColumn(
                name: "TechniqueID",
                table: "ArtPieces");

            migrationBuilder.AddColumn<string>(
                name: "CodeName",
                table: "Techniques",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Techniques",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Technique",
                table: "ArtPieces",
                nullable: false,
                defaultValue: "");
        }
    }
}

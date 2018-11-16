using Microsoft.EntityFrameworkCore.Migrations;

namespace UFArt.Migrations
{
    public partial class ModifiedNews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Header",
                table: "News");

            migrationBuilder.DropColumn(
                name: "Text",
                table: "News");

            migrationBuilder.AddColumn<int>(
                name: "HeaderId",
                table: "News",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TextId",
                table: "News",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_News_HeaderId",
                table: "News",
                column: "HeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_News_TextId",
                table: "News",
                column: "TextId");

            migrationBuilder.AddForeignKey(
                name: "FK_News_TextAssets_HeaderId",
                table: "News",
                column: "HeaderId",
                principalTable: "TextAssets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_News_TextAssets_TextId",
                table: "News",
                column: "TextId",
                principalTable: "TextAssets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_News_TextAssets_HeaderId",
                table: "News");

            migrationBuilder.DropForeignKey(
                name: "FK_News_TextAssets_TextId",
                table: "News");

            migrationBuilder.DropIndex(
                name: "IX_News_HeaderId",
                table: "News");

            migrationBuilder.DropIndex(
                name: "IX_News_TextId",
                table: "News");

            migrationBuilder.DropColumn(
                name: "HeaderId",
                table: "News");

            migrationBuilder.DropColumn(
                name: "TextId",
                table: "News");

            migrationBuilder.AddColumn<string>(
                name: "Header",
                table: "News",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Text",
                table: "News",
                nullable: false,
                defaultValue: "");
        }
    }
}

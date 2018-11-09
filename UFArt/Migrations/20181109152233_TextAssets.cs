using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UFArt.Migrations
{
    public partial class TextAssets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "News",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.CreateTable(
                name: "TextAssets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Key = table.Column<string>(nullable: true),
                    Value_pl = table.Column<string>(nullable: true),
                    Value_en = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TextAssets", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TextAssets");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "News",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}

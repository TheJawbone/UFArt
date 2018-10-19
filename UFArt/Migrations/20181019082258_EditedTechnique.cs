using Microsoft.EntityFrameworkCore.Migrations;

namespace UFArt.Migrations
{
    public partial class EditedTechnique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CodeName",
                table: "Techniques",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodeName",
                table: "Techniques");
        }
    }
}

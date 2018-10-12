using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UFArt.Migrations
{
    public partial class M1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Technique",
                table: "Paintings");

            migrationBuilder.AddColumn<int>(
                name: "TechniqueID",
                table: "Paintings",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Techniques",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Techniques", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Paintings_TechniqueID",
                table: "Paintings",
                column: "TechniqueID");

            migrationBuilder.AddForeignKey(
                name: "FK_Paintings_Techniques_TechniqueID",
                table: "Paintings",
                column: "TechniqueID",
                principalTable: "Techniques",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Paintings_Techniques_TechniqueID",
                table: "Paintings");

            migrationBuilder.DropTable(
                name: "Techniques");

            migrationBuilder.DropIndex(
                name: "IX_Paintings_TechniqueID",
                table: "Paintings");

            migrationBuilder.DropColumn(
                name: "TechniqueID",
                table: "Paintings");

            migrationBuilder.AddColumn<string>(
                name: "Technique",
                table: "Paintings",
                nullable: true);
        }
    }
}

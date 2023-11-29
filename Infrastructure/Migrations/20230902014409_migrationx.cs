using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class migrationx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "AspNetUsers",
                newName: "Lastname");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "AspNetUsers",
                newName: "Firstname");

            migrationBuilder.AddColumn<int>(
                name: "ParentSectionId",
                table: "Sections",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sections_ParentSectionId",
                table: "Sections",
                column: "ParentSectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_Sections_ParentSectionId",
                table: "Sections",
                column: "ParentSectionId",
                principalTable: "Sections",
                principalColumn: "SectionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sections_Sections_ParentSectionId",
                table: "Sections");

            migrationBuilder.DropIndex(
                name: "IX_Sections_ParentSectionId",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "ParentSectionId",
                table: "Sections");

            migrationBuilder.RenameColumn(
                name: "Lastname",
                table: "AspNetUsers",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "Firstname",
                table: "AspNetUsers",
                newName: "FirstName");
        }
    }
}

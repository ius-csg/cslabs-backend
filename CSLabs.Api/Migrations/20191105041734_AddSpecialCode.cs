using Microsoft.EntityFrameworkCore.Migrations;

namespace CSLabs.Api.Migrations
{
    public partial class AddSpecialCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "special_code",
                table: "modules",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_modules_special_code",
                table: "modules",
                column: "special_code",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_modules_special_code",
                table: "modules");

            migrationBuilder.DropColumn(
                name: "special_code",
                table: "modules");
        }
    }
}

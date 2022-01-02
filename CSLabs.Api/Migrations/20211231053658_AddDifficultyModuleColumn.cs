using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSLabs.Api.Migrations
{
    public partial class AddDifficultyModuleColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "difficulty",
                table: "modules",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

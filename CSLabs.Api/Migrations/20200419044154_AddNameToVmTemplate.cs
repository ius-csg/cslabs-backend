using Microsoft.EntityFrameworkCore.Migrations;

namespace CSLabs.Api.Migrations
{
    public partial class AddNameToVmTemplate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "vm_template",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "name",
                table: "vm_template");
        }
    }
}

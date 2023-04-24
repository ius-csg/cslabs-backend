using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSLabs.Api.Migrations
{
    public partial class UserLabVmRunningColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "running",
                table: "user_lab_vms",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "ix_user_lab_vms_running",
                table: "user_lab_vms",
                column: "running");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_user_lab_vms_running",
                table: "user_lab_vms");

            migrationBuilder.DropColumn(
                name: "running",
                table: "user_lab_vms");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace CSLabs.Api.Migrations
{
    public partial class FixLabVmUniqueness : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_lab_vms_name",
                table: "lab_vms");

            migrationBuilder.CreateIndex(
                name: "ix_lab_vms_name_lab_id",
                table: "lab_vms",
                columns: new[] { "name", "lab_id" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_lab_vms_name_lab_id",
                table: "lab_vms");

            migrationBuilder.CreateIndex(
                name: "ix_lab_vms_name",
                table: "lab_vms",
                column: "name",
                unique: true);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace CSLabs.Api.Migrations
{
    public partial class AddVmTemplateToUserLabVmModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "vm_template_id",
                table: "user_lab_vms",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "ix_user_lab_vms_vm_template_id",
                table: "user_lab_vms",
                column: "vm_template_id");

            migrationBuilder.AddForeignKey(
                name: "fk_user_lab_vms_vm_template_vm_template_id",
                table: "user_lab_vms",
                column: "vm_template_id",
                principalTable: "vm_template",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_user_lab_vms_vm_template_vm_template_id",
                table: "user_lab_vms");

            migrationBuilder.DropIndex(
                name: "ix_user_lab_vms_vm_template_id",
                table: "user_lab_vms");

            migrationBuilder.DropColumn(
                name: "vm_template_id",
                table: "user_lab_vms");
        }
    }
}

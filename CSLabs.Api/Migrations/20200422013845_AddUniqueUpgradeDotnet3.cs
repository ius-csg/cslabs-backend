using Microsoft.EntityFrameworkCore.Migrations;

namespace CSLabs.Api.Migrations
{
    public partial class AddUniqueUpgradeDotnet3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_user_lab_vms_hypervisor_vm_templates_hypervisor_vm_template_~",
                table: "user_lab_vms");

            migrationBuilder.DropForeignKey(
                name: "fk_vm_interface_instances_vm_interface_templates_vm_interface_t~",
                table: "vm_interface_instances");

            migrationBuilder.DropIndex(
                name: "ix_labs_name",
                table: "labs");

            migrationBuilder.CreateIndex(
                name: "ix_labs_name_module_id",
                table: "labs",
                columns: new[] { "name", "module_id" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_user_lab_vms_hypervisor_vm_templates_hypervisor_vm_template_",
                table: "user_lab_vms",
                column: "hypervisor_vm_template_id",
                principalTable: "hypervisor_vm_templates",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_vm_interface_instances_vm_interface_templates_vm_interface_t",
                table: "vm_interface_instances",
                column: "vm_interface_template_id",
                principalTable: "vm_interface_templates",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_user_lab_vms_hypervisor_vm_templates_hypervisor_vm_template_",
                table: "user_lab_vms");

            migrationBuilder.DropForeignKey(
                name: "fk_vm_interface_instances_vm_interface_templates_vm_interface_t",
                table: "vm_interface_instances");

            migrationBuilder.DropIndex(
                name: "ix_labs_name_module_id",
                table: "labs");

            migrationBuilder.CreateIndex(
                name: "ix_labs_name",
                table: "labs",
                column: "name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_user_lab_vms_hypervisor_vm_templates_hypervisor_vm_template_~",
                table: "user_lab_vms",
                column: "hypervisor_vm_template_id",
                principalTable: "hypervisor_vm_templates",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_vm_interface_instances_vm_interface_templates_vm_interface_t~",
                table: "vm_interface_instances",
                column: "vm_interface_template_id",
                principalTable: "vm_interface_templates",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

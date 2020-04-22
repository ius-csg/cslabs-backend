using Microsoft.EntityFrameworkCore.Migrations;

namespace CSLabs.Api.Migrations
{
    public partial class FixBridgeInstanceModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_bridge_instances_user_lab_vms_user_lab_vm_id",
                table: "bridge_instances");

            migrationBuilder.DropIndex(
                name: "ix_bridge_instances_user_lab_vm_id",
                table: "bridge_instances");

            migrationBuilder.DropIndex(
                name: "ix_bridge_instances_user_lab_vm_id_hypervisor_interface_id",
                table: "bridge_instances");

            migrationBuilder.DropColumn(
                name: "user_lab_vm_id",
                table: "bridge_instances");

            migrationBuilder.CreateIndex(
                name: "ix_bridge_instances_user_lab_id_hypervisor_interface_id",
                table: "bridge_instances",
                columns: new[] { "user_lab_id", "hypervisor_interface_id" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_bridge_instances_user_lab_id_hypervisor_interface_id",
                table: "bridge_instances");

            migrationBuilder.AddColumn<int>(
                name: "user_lab_vm_id",
                table: "bridge_instances",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "ix_bridge_instances_user_lab_vm_id",
                table: "bridge_instances",
                column: "user_lab_vm_id");

            migrationBuilder.CreateIndex(
                name: "ix_bridge_instances_user_lab_vm_id_hypervisor_interface_id",
                table: "bridge_instances",
                columns: new[] { "user_lab_vm_id", "hypervisor_interface_id" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_bridge_instances_user_lab_vms_user_lab_vm_id",
                table: "bridge_instances",
                column: "user_lab_vm_id",
                principalTable: "user_lab_vms",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

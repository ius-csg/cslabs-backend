using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CSLabs.Api.Migrations
{
    public partial class AddInterfacesAndRouters : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_user_lab_vms_lab_vms_lab_vm_id",
                table: "user_lab_vms");

            migrationBuilder.DropForeignKey(
                name: "fk_user_lab_vms_vm_template_vm_template_id",
                table: "user_lab_vms");

            migrationBuilder.AlterColumn<int>(
                name: "vm_template_id",
                table: "user_lab_vms",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "lab_vm_id",
                table: "user_lab_vms",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<bool>(
                name: "is_root_router",
                table: "user_lab_vms",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "hypervisor_network_interfaces",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    interface_id = table.Column<int>(nullable: false),
                    user_lab_vm_id = table.Column<int>(nullable: false),
                    user_lab_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_hypervisor_network_interfaces", x => x.id);
                    table.ForeignKey(
                        name: "fk_hypervisor_network_interfaces_user_labs_user_lab_id",
                        column: x => x.user_lab_id,
                        principalTable: "user_labs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_hypervisor_network_interfaces_user_lab_vms_user_lab_vm_id",
                        column: x => x.user_lab_vm_id,
                        principalTable: "user_lab_vms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_hypervisor_network_interfaces_interface_id",
                table: "hypervisor_network_interfaces",
                column: "interface_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_hypervisor_network_interfaces_user_lab_id",
                table: "hypervisor_network_interfaces",
                column: "user_lab_id");

            migrationBuilder.CreateIndex(
                name: "ix_hypervisor_network_interfaces_user_lab_vm_id",
                table: "hypervisor_network_interfaces",
                column: "user_lab_vm_id");

            migrationBuilder.AddForeignKey(
                name: "fk_user_lab_vms_lab_vms_lab_vm_id",
                table: "user_lab_vms",
                column: "lab_vm_id",
                principalTable: "lab_vms",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_user_lab_vms_vm_template_vm_template_id",
                table: "user_lab_vms",
                column: "vm_template_id",
                principalTable: "vm_template",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_user_lab_vms_lab_vms_lab_vm_id",
                table: "user_lab_vms");

            migrationBuilder.DropForeignKey(
                name: "fk_user_lab_vms_vm_template_vm_template_id",
                table: "user_lab_vms");

            migrationBuilder.DropTable(
                name: "hypervisor_network_interfaces");

            migrationBuilder.DropColumn(
                name: "is_root_router",
                table: "user_lab_vms");

            migrationBuilder.AlterColumn<int>(
                name: "vm_template_id",
                table: "user_lab_vms",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "lab_vm_id",
                table: "user_lab_vms",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "fk_user_lab_vms_lab_vms_lab_vm_id",
                table: "user_lab_vms",
                column: "lab_vm_id",
                principalTable: "lab_vms",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_lab_vms_vm_template_vm_template_id",
                table: "user_lab_vms",
                column: "vm_template_id",
                principalTable: "vm_template",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

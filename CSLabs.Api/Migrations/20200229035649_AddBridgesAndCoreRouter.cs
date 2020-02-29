using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CSLabs.Api.Migrations
{
    public partial class AddBridgesAndCoreRouter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_core_router",
                table: "vm_template",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_core_router",
                table: "user_lab_vms",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "hypervisor_bridge_instances",
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
                    table.PrimaryKey("pk_hypervisor_bridge_instances", x => x.id);
                    table.ForeignKey(
                        name: "fk_hypervisor_bridge_instances_user_labs_user_lab_id",
                        column: x => x.user_lab_id,
                        principalTable: "user_labs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_hypervisor_bridge_instances_user_lab_vms_user_lab_vm_id",
                        column: x => x.user_lab_vm_id,
                        principalTable: "user_lab_vms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_hypervisor_bridge_instances_interface_id",
                table: "hypervisor_bridge_instances",
                column: "interface_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_hypervisor_bridge_instances_user_lab_id",
                table: "hypervisor_bridge_instances",
                column: "user_lab_id");

            migrationBuilder.CreateIndex(
                name: "ix_hypervisor_bridge_instances_user_lab_vm_id",
                table: "hypervisor_bridge_instances",
                column: "user_lab_vm_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "hypervisor_bridge_instances");

            migrationBuilder.DropColumn(
                name: "is_core_router",
                table: "vm_template");

            migrationBuilder.DropColumn(
                name: "is_core_router",
                table: "user_lab_vms");
        }
    }
}

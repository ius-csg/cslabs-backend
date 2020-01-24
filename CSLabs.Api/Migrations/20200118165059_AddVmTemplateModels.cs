using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CSLabs.Api.Migrations
{
    public partial class AddVmTemplateModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "template_proxmox_vm_id",
                table: "lab_vms");

            migrationBuilder.CreateTable(
                name: "vm_template",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    hypervisor_node_id = table.Column<int>(nullable: false),
                    template_vm_id = table.Column<int>(nullable: false),
                    lab_vm_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_vm_template", x => x.id);
                    table.ForeignKey(
                        name: "fk_vm_template_hypervisor_nodes_hypervisor_node_id",
                        column: x => x.hypervisor_node_id,
                        principalTable: "hypervisor_nodes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_vm_template_lab_vms_lab_vm_id",
                        column: x => x.lab_vm_id,
                        principalTable: "lab_vms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_vm_template_hypervisor_node_id",
                table: "vm_template",
                column: "hypervisor_node_id");

            migrationBuilder.CreateIndex(
                name: "ix_vm_template_lab_vm_id",
                table: "vm_template",
                column: "lab_vm_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "vm_template");

            migrationBuilder.AddColumn<int>(
                name: "template_proxmox_vm_id",
                table: "lab_vms",
                nullable: false,
                defaultValue: 0);
        }
    }
}

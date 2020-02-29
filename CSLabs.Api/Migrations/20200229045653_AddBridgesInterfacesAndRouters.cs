using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CSLabs.Api.Migrations
{
    public partial class AddBridgesInterfacesAndRouters : Migration
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
                name: "bridge_templates",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(nullable: false),
                    uuid = table.Column<string>(nullable: false),
                    lab_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_bridge_templates", x => x.id);
                    table.ForeignKey(
                        name: "fk_bridge_templates_labs_lab_id",
                        column: x => x.lab_id,
                        principalTable: "labs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "bridge_instances",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    hypervisor_interface_id = table.Column<int>(nullable: false),
                    user_lab_vm_id = table.Column<int>(nullable: false),
                    bridge_template_id = table.Column<int>(nullable: false),
                    user_lab_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_bridge_instances", x => x.id);
                    table.ForeignKey(
                        name: "fk_bridge_instances_bridge_templates_bridge_template_id",
                        column: x => x.bridge_template_id,
                        principalTable: "bridge_templates",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_bridge_instances_user_labs_user_lab_id",
                        column: x => x.user_lab_id,
                        principalTable: "user_labs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_bridge_instances_user_lab_vms_user_lab_vm_id",
                        column: x => x.user_lab_vm_id,
                        principalTable: "user_lab_vms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "vm_interface_templates",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    interface_number = table.Column<int>(nullable: false),
                    lab_vm_id = table.Column<int>(nullable: false),
                    bridge_template_uuid = table.Column<string>(nullable: false),
                    bridge_template_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_vm_interface_templates", x => x.id);
                    table.ForeignKey(
                        name: "fk_vm_interface_templates_bridge_templates_bridge_template_id",
                        column: x => x.bridge_template_id,
                        principalTable: "bridge_templates",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_vm_interface_templates_lab_vms_lab_vm_id",
                        column: x => x.lab_vm_id,
                        principalTable: "lab_vms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "vm_interface_instances",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    user_lab_vm_id = table.Column<int>(nullable: false),
                    bridge_instance_id = table.Column<int>(nullable: false),
                    vm_interface_template_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_vm_interface_instances", x => x.id);
                    table.ForeignKey(
                        name: "fk_vm_interface_instances_bridge_instances_bridge_instance_id",
                        column: x => x.bridge_instance_id,
                        principalTable: "bridge_instances",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_vm_interface_instances_user_lab_vms_user_lab_vm_id",
                        column: x => x.user_lab_vm_id,
                        principalTable: "user_lab_vms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_vm_interface_instances_vm_interface_templates_vm_interface_t~",
                        column: x => x.vm_interface_template_id,
                        principalTable: "vm_interface_templates",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_bridge_instances_bridge_template_id",
                table: "bridge_instances",
                column: "bridge_template_id");

            migrationBuilder.CreateIndex(
                name: "ix_bridge_instances_user_lab_id",
                table: "bridge_instances",
                column: "user_lab_id");

            migrationBuilder.CreateIndex(
                name: "ix_bridge_instances_user_lab_vm_id",
                table: "bridge_instances",
                column: "user_lab_vm_id");

            migrationBuilder.CreateIndex(
                name: "ix_bridge_instances_user_lab_vm_id_hypervisor_interface_id",
                table: "bridge_instances",
                columns: new[] { "user_lab_vm_id", "hypervisor_interface_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_bridge_templates_lab_id",
                table: "bridge_templates",
                column: "lab_id");

            migrationBuilder.CreateIndex(
                name: "ix_bridge_templates_lab_id_name",
                table: "bridge_templates",
                columns: new[] { "lab_id", "name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_bridge_templates_lab_id_uuid",
                table: "bridge_templates",
                columns: new[] { "lab_id", "uuid" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_vm_interface_instances_bridge_instance_id",
                table: "vm_interface_instances",
                column: "bridge_instance_id");

            migrationBuilder.CreateIndex(
                name: "ix_vm_interface_instances_user_lab_vm_id",
                table: "vm_interface_instances",
                column: "user_lab_vm_id");

            migrationBuilder.CreateIndex(
                name: "ix_vm_interface_instances_vm_interface_template_id",
                table: "vm_interface_instances",
                column: "vm_interface_template_id");

            migrationBuilder.CreateIndex(
                name: "ix_vm_interface_templates_bridge_template_id",
                table: "vm_interface_templates",
                column: "bridge_template_id");

            migrationBuilder.CreateIndex(
                name: "ix_vm_interface_templates_lab_vm_id",
                table: "vm_interface_templates",
                column: "lab_vm_id");

            migrationBuilder.CreateIndex(
                name: "ix_vm_interface_templates_interface_number_lab_vm_id",
                table: "vm_interface_templates",
                columns: new[] { "interface_number", "lab_vm_id" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "vm_interface_instances");

            migrationBuilder.DropTable(
                name: "bridge_instances");

            migrationBuilder.DropTable(
                name: "vm_interface_templates");

            migrationBuilder.DropTable(
                name: "bridge_templates");

            migrationBuilder.DropColumn(
                name: "is_core_router",
                table: "vm_template");

            migrationBuilder.DropColumn(
                name: "is_core_router",
                table: "user_lab_vms");
        }
    }
}

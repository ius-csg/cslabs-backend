using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CSLabs.Api.Migrations
{
    public partial class AddHypervisorVmTemplate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_user_lab_vms_vm_template_vm_template_id",
                table: "user_lab_vms");
            
            migrationBuilder.DropForeignKey(
                name: "fk_vm_template_hypervisor_nodes_hypervisor_node_id",
                table: "vm_template");
            
            migrationBuilder.DropForeignKey(
                name: "fk_vm_template_lab_vms_lab_vm_id",
                table: "vm_template");
            
            migrationBuilder.DropIndex(
                name: "ix_vm_template_hypervisor_node_id",
                table: "vm_template");
            
            migrationBuilder.DropIndex(
                name: "ix_vm_template_lab_vm_id",
                table: "vm_template");
            
            
            
            migrationBuilder.RenameColumn(
                name: "vm_template_id",
                table: "user_lab_vms",
                newName: "hypervisor_vm_template_id");
            
            migrationBuilder.RenameIndex(
                name: "ix_user_lab_vms_vm_template_id",
                table: "user_lab_vms",
                newName: "ix_user_lab_vms_hypervisor_vm_template_id");
            
            migrationBuilder.AddColumn<int>(
                name: "vm_template_id",
                table: "lab_vms",
                nullable: false,
                defaultValue: 0);
            
            migrationBuilder.CreateTable(
                name: "hypervisor_vm_templates",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    hypervisor_node_id = table.Column<int>(nullable: false),
                    template_vm_id = table.Column<int>(nullable: false),
                    vm_template_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_hypervisor_vm_templates", x => x.id);
                    table.ForeignKey(
                        name: "fk_hypervisor_vm_templates_hypervisor_nodes_hypervisor_node_id",
                        column: x => x.hypervisor_node_id,
                        principalTable: "hypervisor_nodes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_hypervisor_vm_templates_vm_template_vm_template_id",
                        column: x => x.vm_template_id,
                        principalTable: "vm_template",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });
            
            
            migrationBuilder.Sql(@"
                INSERT INTO hypervisor_vm_templates 
                    (template_vm_id, vm_template_id, hypervisor_node_id)
                SELECT template_vm_id, id AS vm_template_id, hypervisor_node_id FROM vm_template
            ");
            //
            migrationBuilder.Sql(@"
                UPDATE lab_vms
                    LEFT JOIN vm_template ON lab_vms.id = vm_template.lab_vm_id
                    SET vm_template_id = vm_template.id;
            ");
        
            migrationBuilder.Sql(@"
                UPDATE user_lab_vms vm
                    LEFT JOIN vm_template ON vm.hypervisor_vm_template_id = vm_template.id
                    LEFT JOIN hypervisor_vm_templates ON vm_template.id = hypervisor_vm_templates.vm_template_id
                    SET hypervisor_vm_template_id = hypervisor_vm_templates.id;
            ");
            
            migrationBuilder.DropColumn(
                name: "hypervisor_node_id",
                table: "vm_template");

            migrationBuilder.DropColumn(
                name: "lab_vm_id",
                table: "vm_template");

            migrationBuilder.DropColumn(
                name: "template_vm_id",
                table: "vm_template");

            migrationBuilder.CreateIndex(
                name: "ix_lab_vms_vm_template_id",
                table: "lab_vms",
                column: "vm_template_id");

            migrationBuilder.CreateIndex(
                name: "ix_hypervisor_vm_templates_hypervisor_node_id",
                table: "hypervisor_vm_templates",
                column: "hypervisor_node_id");

            migrationBuilder.CreateIndex(
                name: "ix_hypervisor_vm_templates_vm_template_id",
                table: "hypervisor_vm_templates",
                column: "vm_template_id");

            migrationBuilder.AddForeignKey(
                name: "fk_lab_vms_vm_template_vm_template_id",
                table: "lab_vms",
                column: "vm_template_id",
                principalTable: "vm_template",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_lab_vms_hypervisor_vm_templates_hypervisor_vm_template_~",
                table: "user_lab_vms",
                column: "hypervisor_vm_template_id",
                principalTable: "hypervisor_vm_templates",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_lab_vms_vm_template_vm_template_id",
                table: "lab_vms");

            migrationBuilder.DropForeignKey(
                name: "fk_user_lab_vms_hypervisor_vm_templates_hypervisor_vm_template_~",
                table: "user_lab_vms");

            migrationBuilder.DropTable(
                name: "hypervisor_vm_templates");

            migrationBuilder.DropIndex(
                name: "ix_lab_vms_vm_template_id",
                table: "lab_vms");

            migrationBuilder.DropColumn(
                name: "vm_template_id",
                table: "lab_vms");

            migrationBuilder.RenameColumn(
                name: "hypervisor_vm_template_id",
                table: "user_lab_vms",
                newName: "vm_template_id");

            migrationBuilder.RenameIndex(
                name: "ix_user_lab_vms_hypervisor_vm_template_id",
                table: "user_lab_vms",
                newName: "ix_user_lab_vms_vm_template_id");

            migrationBuilder.AddColumn<int>(
                name: "hypervisor_node_id",
                table: "vm_template",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "lab_vm_id",
                table: "vm_template",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "template_vm_id",
                table: "vm_template",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "ix_vm_template_hypervisor_node_id",
                table: "vm_template",
                column: "hypervisor_node_id");

            migrationBuilder.CreateIndex(
                name: "ix_vm_template_lab_vm_id",
                table: "vm_template",
                column: "lab_vm_id");

            migrationBuilder.AddForeignKey(
                name: "fk_user_lab_vms_vm_template_vm_template_id",
                table: "user_lab_vms",
                column: "vm_template_id",
                principalTable: "vm_template",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_vm_template_hypervisor_nodes_hypervisor_node_id",
                table: "vm_template",
                column: "hypervisor_node_id",
                principalTable: "hypervisor_nodes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_vm_template_lab_vms_lab_vm_id",
                table: "vm_template",
                column: "lab_vm_id",
                principalTable: "lab_vms",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace CSLabs.Api.Migrations
{
    public partial class AddHypervisorNodeToUserLab : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                INSERT INTO `cslabs_backend`.`hypervisors`
                (
                `host`,
                `user_name`,
                `password`,
                `no_vnc_url`)
                VALUES
                (
                '192.168.5.68',
                'root',
                'changeme',
                'wss://192.168.5.68:8006/api2/json/nodes/{node}/qemu/{vm}/vncwebsocket'
                );
            ");
            
            migrationBuilder.Sql(@"
                INSERT INTO `cslabs_backend`.`hypervisor_nodes`
                (
                `name`,
                `hypervisor_id`)
                VALUES
                ('testenv1', 1);

            ");
            migrationBuilder.AddColumn<int>(
                name: "hypervisor_node_id",
                table: "user_labs",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "ix_user_labs_hypervisor_node_id",
                table: "user_labs",
                column: "hypervisor_node_id");

            migrationBuilder.AddForeignKey(
                name: "fk_user_labs_hypervisor_nodes_hypervisor_node_id",
                table: "user_labs",
                column: "hypervisor_node_id",
                principalTable: "hypervisor_nodes",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_user_labs_hypervisor_nodes_hypervisor_node_id",
                table: "user_labs");

            migrationBuilder.DropIndex(
                name: "ix_user_labs_hypervisor_node_id",
                table: "user_labs");

            migrationBuilder.DropColumn(
                name: "hypervisor_node_id",
                table: "user_labs");
        }
    }
}

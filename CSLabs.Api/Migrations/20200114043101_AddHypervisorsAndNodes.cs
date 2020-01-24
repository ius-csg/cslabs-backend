using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CSLabs.Api.Migrations
{
    public partial class AddHypervisorsAndNodes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "hypervisors",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    host = table.Column<string>(nullable: true),
                    user_name = table.Column<string>(nullable: false),
                    password = table.Column<string>(nullable: false),
                    no_vnc_url = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_hypervisors", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "hypervisor_nodes",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(nullable: false),
                    hypervisor_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_hypervisor_nodes", x => x.id);
                    table.ForeignKey(
                        name: "fk_hypervisor_nodes_hypervisors_hypervisor_id",
                        column: x => x.hypervisor_id,
                        principalTable: "hypervisors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_hypervisor_nodes_hypervisor_id",
                table: "hypervisor_nodes",
                column: "hypervisor_id");

            migrationBuilder.CreateIndex(
                name: "ix_hypervisor_nodes_name_hypervisor_id",
                table: "hypervisor_nodes",
                columns: new[] { "name", "hypervisor_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_hypervisors_host",
                table: "hypervisors",
                column: "host",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_hypervisors_no_vnc_url",
                table: "hypervisors",
                column: "no_vnc_url",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "hypervisor_nodes");

            migrationBuilder.DropTable(
                name: "hypervisors");
        }
    }
}

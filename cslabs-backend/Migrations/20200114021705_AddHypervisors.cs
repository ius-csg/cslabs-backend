using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CSLabsBackend.Migrations
{
    public partial class AddHypervisors : Migration
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
                name: "hypervisors");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace CSLabs.Api.Migrations
{
    public partial class AddDelayedUserLabInstantiation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "hypervisor_node_id",
                table: "user_labs",
                nullable: true,
                oldClrType: typeof(int));
            
            migrationBuilder.Sql(@"
                UPDATE user_labs
                SET status = 'Started'
                WHERE status = 'ON'
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "hypervisor_node_id",
                table: "user_labs",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace CSLabs.Api.Migrations
{
    public partial class VerfiedforUseremail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "verified",
                table: "users",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "verified",
                table: "users");
        }
    }
}

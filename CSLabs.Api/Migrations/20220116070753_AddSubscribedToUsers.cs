using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSLabs.Api.Migrations
{
    public partial class AddSubscribedToUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "subscribed",
                table: "users",
                nullable: false,
                defaultValue: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "subscribed",
                table: "users");
        }
    }
}

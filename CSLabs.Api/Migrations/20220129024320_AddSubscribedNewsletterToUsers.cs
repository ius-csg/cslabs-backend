using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSLabs.Api.Migrations
{
    public partial class AddSubscribedNewsletterToUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "subscribed_newsletter",
                table: "users",
                nullable: false,
                defaultValue: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "subscribed_newsletter",
                table: "users");
        }
    }
}

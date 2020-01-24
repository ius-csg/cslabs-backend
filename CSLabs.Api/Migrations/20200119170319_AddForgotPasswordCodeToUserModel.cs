using Microsoft.EntityFrameworkCore.Migrations;

namespace CSLabs.Api.Migrations
{
    public partial class AddForgotPasswordCodeToUserModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "password_recovery_code",
                table: "users",
                type: "VARCHAR(100)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_users_password_recovery_code",
                table: "users",
                column: "password_recovery_code",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_users_password_recovery_code",
                table: "users");

            migrationBuilder.DropColumn(
                name: "password_recovery_code",
                table: "users");
        }
    }
}

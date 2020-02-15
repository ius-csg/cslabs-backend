using Microsoft.EntityFrameworkCore.Migrations;

namespace CSLabs.Api.Migrations
{
    public partial class RemoveSchoolAndPersonalEmail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_users_school_email",
                table: "users");

            migrationBuilder.DropColumn(
                name: "personal_email_verification_code",
                table: "users");

            migrationBuilder.Sql(@"
                UPDATE users
                SET personal_email = school_email
                WHERE personal_email IS NULL
            ");

            migrationBuilder.DropColumn(
                name: "school_email",
                table: "users");

            migrationBuilder.RenameColumn(
                name: "school_email_verification_code",
                table: "users",
                newName: "email_verification_code");

            migrationBuilder.RenameColumn(
                name: "personal_email",
                table: "users",
                newName: "email");

            migrationBuilder.RenameIndex(
                name: "ix_users_personal_email",
                table: "users",
                newName: "ix_users_email");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "email_verification_code",
                table: "users",
                newName: "school_email_verification_code");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "users",
                newName: "personal_email");

            migrationBuilder.RenameIndex(
                name: "ix_users_email",
                table: "users",
                newName: "ix_users_personal_email");

            migrationBuilder.AddColumn<string>(
                name: "personal_email_verification_code",
                table: "users",
                type: "VARCHAR(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "school_email",
                table: "users",
                type: "VARCHAR(100)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_users_school_email",
                table: "users",
                column: "school_email",
                unique: true);
        }
    }
}

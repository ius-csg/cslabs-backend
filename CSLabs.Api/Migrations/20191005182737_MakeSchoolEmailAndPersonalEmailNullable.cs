using Microsoft.EntityFrameworkCore.Migrations;

namespace CSLabs.Api.Migrations
{
    public partial class MakeSchoolEmailAndPersonalEmailNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "school_email",
                table: "users",
                type: "VARCHAR(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(100)");

            migrationBuilder.AlterColumn<string>(
                name: "personal_email",
                table: "users",
                type: "VARCHAR(45)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(45)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "school_email",
                table: "users",
                type: "VARCHAR(100)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "personal_email",
                table: "users",
                type: "VARCHAR(45)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(45)",
                oldNullable: true);
        }
    }
}

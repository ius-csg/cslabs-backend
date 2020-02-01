using Microsoft.EntityFrameworkCore.Migrations;

namespace CSLabs.Api.Migrations
{
    public partial class AddModuleConstraints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "short_name",
                table: "modules",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "modules",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.CreateIndex(
                name: "ix_modules_name",
                table: "modules",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_modules_short_name",
                table: "modules",
                column: "short_name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_modules_name",
                table: "modules");

            migrationBuilder.DropIndex(
                name: "ix_modules_short_name",
                table: "modules");

            migrationBuilder.AlterColumn<string>(
                name: "short_name",
                table: "modules",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "modules",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}

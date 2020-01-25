using Microsoft.EntityFrameworkCore.Migrations;

namespace CSLabs.Api.Migrations
{
    public partial class RemoveShortNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_user_lab_vms_labs_lab_id",
                table: "user_lab_vms");

            migrationBuilder.DropIndex(
                name: "ix_user_lab_vms_lab_id",
                table: "user_lab_vms");

            migrationBuilder.DropIndex(
                name: "ix_modules_short_name",
                table: "modules");

            migrationBuilder.DropIndex(
                name: "ix_badges_short_name",
                table: "badges");

            migrationBuilder.DropColumn(
                name: "lab_id",
                table: "user_lab_vms");

            migrationBuilder.DropColumn(
                name: "short_name",
                table: "modules");

            migrationBuilder.DropColumn(
                name: "short_name",
                table: "badges");

            migrationBuilder.CreateIndex(
                name: "ix_user_lab_vms_lab_vm_id",
                table: "user_lab_vms",
                column: "lab_vm_id");

            migrationBuilder.AddForeignKey(
                name: "fk_user_lab_vms_lab_vms_lab_vm_id",
                table: "user_lab_vms",
                column: "lab_vm_id",
                principalTable: "lab_vms",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_user_lab_vms_lab_vms_lab_vm_id",
                table: "user_lab_vms");

            migrationBuilder.DropIndex(
                name: "ix_user_lab_vms_lab_vm_id",
                table: "user_lab_vms");

            migrationBuilder.AddColumn<int>(
                name: "lab_id",
                table: "user_lab_vms",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "short_name",
                table: "modules",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "short_name",
                table: "badges",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "ix_user_lab_vms_lab_id",
                table: "user_lab_vms",
                column: "lab_id");

            migrationBuilder.CreateIndex(
                name: "ix_modules_short_name",
                table: "modules",
                column: "short_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_badges_short_name",
                table: "badges",
                column: "short_name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_user_lab_vms_labs_lab_id",
                table: "user_lab_vms",
                column: "lab_id",
                principalTable: "labs",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

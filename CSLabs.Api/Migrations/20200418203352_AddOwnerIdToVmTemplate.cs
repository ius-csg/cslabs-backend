using Microsoft.EntityFrameworkCore.Migrations;

namespace CSLabs.Api.Migrations
{
    public partial class AddOwnerIdToVmTemplate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "owner_id",
                table: "vm_template",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_vm_template_owner_id",
                table: "vm_template",
                column: "owner_id");

            migrationBuilder.AddForeignKey(
                name: "fk_vm_template_users_owner_id",
                table: "vm_template",
                column: "owner_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_vm_template_users_owner_id",
                table: "vm_template");

            migrationBuilder.DropIndex(
                name: "ix_vm_template_owner_id",
                table: "vm_template");

            migrationBuilder.DropColumn(
                name: "owner_id",
                table: "vm_template");
        }
    }
}

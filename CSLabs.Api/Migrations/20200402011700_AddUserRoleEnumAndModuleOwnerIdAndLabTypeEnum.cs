using Microsoft.EntityFrameworkCore.Migrations;

namespace CSLabs.Api.Migrations
{
    public partial class AddUserRoleEnumAndModuleOwnerIdAndLabTypeEnum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                UPDATE users u
                    SET u.user_type = IF(
                        u.user_type = 'admin', 'Admin', 
                        IF(u.user_type = 'creator', 'Creator',
                            IF(u.user_type = 'guest', 'Guest', ''
                        )))
            ");
            migrationBuilder.RenameColumn(
                name: "user_type",
                newName: "role",
                table: "users");

            migrationBuilder.Sql(@"
                UPDATE labs l
                    SET l.lab_type = IF(
                        l.lab_type = 'temporary', 'Temporary', 
                        IF(l.lab_type = 'class', 'Class',
                            IF(l.lab_type = 'permanent', 'Permanent', ''
                        )))
            ");
            migrationBuilder.RenameColumn(
                name: "lab_type",
                newName: "type",
                table: "labs");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "labs");

            migrationBuilder.AddColumn<int>(
                name: "owner_id",
                table: "modules",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_core_bridge",
                table: "bridge_templates",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "ix_modules_owner_id",
                table: "modules",
                column: "owner_id");

            migrationBuilder.AddForeignKey(
                name: "fk_modules_users_owner_id",
                table: "modules",
                column: "owner_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_modules_users_owner_id",
                table: "modules");

            migrationBuilder.DropIndex(
                name: "ix_modules_owner_id",
                table: "modules");
            
            migrationBuilder.RenameColumn(
                name: "role",
                newName: "user_type",
                table: "users");

            migrationBuilder.RenameColumn(
                name: "type",
                newName: "lab_type",
                table: "labs");

            migrationBuilder.DropColumn(
                name: "owner_id",
                table: "modules");
            
            migrationBuilder.DropColumn(
                name: "is_core_bridge",
                table: "bridge_templates");
            
            migrationBuilder.AddColumn<int>(
                name: "user_id",
                table: "labs",
                nullable: false,
                defaultValue: 0);
        }
    }
}

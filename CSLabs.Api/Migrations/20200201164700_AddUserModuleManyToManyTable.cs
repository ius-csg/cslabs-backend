using Microsoft.EntityFrameworkCore.Migrations;

namespace CSLabs.Api.Migrations
{
    public partial class AddUserModuleManyToManyTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_user_lab_vms_users_user_id",
                table: "user_lab_vms");

            migrationBuilder.DropForeignKey(
                name: "fk_user_labs_users_user_id",
                table: "user_labs");

            migrationBuilder.DropForeignKey(
                name: "fk_user_modules_users_user_id",
                table: "user_modules");

            migrationBuilder.DropIndex(
                name: "ix_user_modules_user_id_module_id",
                table: "user_modules");

            migrationBuilder.DropIndex(
                name: "ix_user_labs_user_id_lab_id",
                table: "user_labs");

            migrationBuilder.DropIndex(
                name: "ix_user_lab_vms_user_id_lab_vm_id",
                table: "user_lab_vms");

       

            migrationBuilder.AddColumn<string>(
                name: "type",
                table: "modules",
                nullable: false,
                defaultValue: "SingleUser");

            migrationBuilder.CreateTable(
                name: "user_user_module",
                columns: table => new
                {
                    user_id = table.Column<int>(nullable: false),
                    user_module_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_user_module", x => new { x.user_id, x.user_module_id });
                    table.ForeignKey(
                        name: "fk_user_user_module_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_user_module_user_modules_user_module_id",
                        column: x => x.user_module_id,
                        principalTable: "user_modules",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_user_labs_user_module_id_lab_id",
                table: "user_labs",
                columns: new[] { "user_module_id", "lab_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_lab_vms_user_lab_id_lab_vm_id",
                table: "user_lab_vms",
                columns: new[] { "user_lab_id", "lab_vm_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_user_module_user_module_id",
                table: "user_user_module",
                column: "user_module_id");
            
                 
            migrationBuilder.Sql(@"
                INSERT INTO user_user_module
                SELECT user_id, id AS module_id FROM user_modules;
            ");
            
            migrationBuilder.DropColumn(
                name: "user_id",
                table: "user_modules");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "user_labs");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "user_lab_vms");
            
       
            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "user_id",
                table: "user_modules",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "user_id",
                table: "user_labs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "user_id",
                table: "user_lab_vms",
                nullable: false,
                defaultValue: 0);
            
            
            migrationBuilder.Sql(@"
                UPDATE user_modules
                LEFT JOIN user_user_module uum on user_modules.id = uum.user_module_id
                SET user_modules.user_id = uum.user_id;
            ");
            migrationBuilder.Sql(@"
                UPDATE user_labs
                LEFT JOIN user_modules um ON user_labs.user_module_id = um.id
                LEFT JOIN user_user_module uum on um.id = uum.user_module_id
                SET user_labs.user_id = uum.user_id;
            ");
            
            migrationBuilder.Sql(@"
                UPDATE user_lab_vms
                LEFT JOIN user_labs ul ON user_lab_vms.user_lab_id = ul.id
                LEFT JOIN user_modules um ON ul.user_module_id = um.id
                LEFT JOIN user_user_module uum on um.id = uum.user_module_id
                SET user_lab_vms.user_id = uum.user_id;
            ");
            
            migrationBuilder.DropTable(
                name: "user_user_module");

            migrationBuilder.DropIndex(
                name: "ix_user_labs_user_module_id_lab_id",
                table: "user_labs");

            migrationBuilder.DropIndex(
                name: "ix_user_lab_vms_user_lab_id_lab_vm_id",
                table: "user_lab_vms");

            migrationBuilder.DropColumn(
                name: "type",
                table: "modules");
            

            migrationBuilder.CreateIndex(
                name: "ix_user_modules_user_id_module_id",
                table: "user_modules",
                columns: new[] { "user_id", "module_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_labs_user_id_lab_id",
                table: "user_labs",
                columns: new[] { "user_id", "lab_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_lab_vms_user_id_lab_vm_id",
                table: "user_lab_vms",
                columns: new[] { "user_id", "lab_vm_id" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_user_lab_vms_users_user_id",
                table: "user_lab_vms",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_labs_users_user_id",
                table: "user_labs",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_modules_users_user_id",
                table: "user_modules",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

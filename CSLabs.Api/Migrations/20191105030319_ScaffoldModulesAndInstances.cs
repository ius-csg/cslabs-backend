using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CSLabs.Api.Migrations
{
    public partial class ScaffoldModulesAndInstances : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "deleted_at",
                table: "labs");

            migrationBuilder.DropColumn(
                name: "rundeck_create_url",
                table: "labs");

            migrationBuilder.DropColumn(
                name: "rundeck_destroy_url",
                table: "labs");

            migrationBuilder.DropColumn(
                name: "rundeck_restore_snapshot_url",
                table: "labs");

            migrationBuilder.DropColumn(
                name: "rundeck_snapshot_url",
                table: "labs");

            migrationBuilder.DropColumn(
                name: "rundeck_start_url",
                table: "labs");

            migrationBuilder.DropColumn(
                name: "rundeck_stop_url",
                table: "labs");

            migrationBuilder.RenameColumn(
                name: "creator_id",
                table: "labs",
                newName: "user_id");

            migrationBuilder.AlterColumn<string>(
                name: "lab_type",
                table: "labs",
                type: "VARCHAR(45)",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.CreateTable(
                name: "lab_vms",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "UTC_TIMESTAMP()"),
                    updated_at = table.Column<DateTime>(nullable: false, defaultValueSql: "UTC_TIMESTAMP()"),
                    name = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    lab_id = table.Column<int>(nullable: false),
                    template_proxmox_vm_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_lab_vms", x => x.id);
                    table.ForeignKey(
                        name: "fk_lab_vms_labs_lab_id",
                        column: x => x.lab_id,
                        principalTable: "labs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_modules",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "UTC_TIMESTAMP()"),
                    updated_at = table.Column<DateTime>(nullable: false, defaultValueSql: "UTC_TIMESTAMP()"),
                    user_id = table.Column<int>(nullable: false),
                    module_id = table.Column<int>(nullable: false),
                    termination_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_modules", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_modules_modules_module_id",
                        column: x => x.module_id,
                        principalTable: "modules",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_modules_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_labs",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "UTC_TIMESTAMP()"),
                    updated_at = table.Column<DateTime>(nullable: false, defaultValueSql: "UTC_TIMESTAMP()"),
                    user_id = table.Column<int>(nullable: false),
                    user_module_id = table.Column<int>(nullable: false),
                    lab_id = table.Column<int>(nullable: false),
                    status = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_labs", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_labs_labs_lab_id",
                        column: x => x.lab_id,
                        principalTable: "labs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_labs_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_labs_user_modules_user_module_id",
                        column: x => x.user_module_id,
                        principalTable: "user_modules",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_lab_vms",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "UTC_TIMESTAMP()"),
                    updated_at = table.Column<DateTime>(nullable: false, defaultValueSql: "UTC_TIMESTAMP()"),
                    user_id = table.Column<int>(nullable: false),
                    user_lab_id = table.Column<int>(nullable: false),
                    lab_vm_id = table.Column<int>(nullable: false),
                    proxmox_vm_id = table.Column<int>(nullable: false),
                    lab_id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_lab_vms", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_lab_vms_labs_lab_id",
                        column: x => x.lab_id,
                        principalTable: "labs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_user_lab_vms_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_lab_vms_user_labs_user_lab_id",
                        column: x => x.user_lab_id,
                        principalTable: "user_labs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_labs_module_id",
                table: "labs",
                column: "module_id");

            migrationBuilder.CreateIndex(
                name: "ix_lab_vms_lab_id",
                table: "lab_vms",
                column: "lab_id");

            migrationBuilder.CreateIndex(
                name: "ix_lab_vms_name",
                table: "lab_vms",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_lab_vms_lab_id",
                table: "user_lab_vms",
                column: "lab_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_lab_vms_user_lab_id",
                table: "user_lab_vms",
                column: "user_lab_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_lab_vms_user_id_lab_vm_id",
                table: "user_lab_vms",
                columns: new[] { "user_id", "lab_vm_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_labs_lab_id",
                table: "user_labs",
                column: "lab_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_labs_user_module_id",
                table: "user_labs",
                column: "user_module_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_labs_user_id_lab_id",
                table: "user_labs",
                columns: new[] { "user_id", "lab_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_modules_module_id",
                table: "user_modules",
                column: "module_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_modules_user_id_module_id",
                table: "user_modules",
                columns: new[] { "user_id", "module_id" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_labs_modules_module_id",
                table: "labs",
                column: "module_id",
                principalTable: "modules",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_labs_modules_module_id",
                table: "labs");

            migrationBuilder.DropTable(
                name: "lab_vms");

            migrationBuilder.DropTable(
                name: "user_lab_vms");

            migrationBuilder.DropTable(
                name: "user_labs");

            migrationBuilder.DropTable(
                name: "user_modules");

            migrationBuilder.DropIndex(
                name: "ix_labs_module_id",
                table: "labs");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "labs",
                newName: "creator_id");

            migrationBuilder.AlterColumn<int>(
                name: "lab_type",
                table: "labs",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(45)");

            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_at",
                table: "labs",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "rundeck_create_url",
                table: "labs",
                type: "VARCHAR(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "rundeck_destroy_url",
                table: "labs",
                type: "VARCHAR(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "rundeck_restore_snapshot_url",
                table: "labs",
                type: "VARCHAR(45)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "rundeck_snapshot_url",
                table: "labs",
                type: "VARCHAR(45)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "rundeck_start_url",
                table: "labs",
                type: "VARCHAR(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "rundeck_stop_url",
                table: "labs",
                type: "VARCHAR(100)",
                nullable: true);
        }
    }
}

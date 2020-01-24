using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CSLabs.Api.Migrations
{
    public partial class AddUserAndLabModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "labs",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "VARCHAR(100)", nullable: true),
                    lab_type = table.Column<int>(nullable: false),
                    module_id = table.Column<int>(nullable: false),
                    creator_id = table.Column<int>(nullable: false),
                    lab_difficulty = table.Column<int>(nullable: false),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "UTC_TIMESTAMP()"),
                    updated_at = table.Column<DateTime>(nullable: false),
                    deleted_at = table.Column<DateTime>(nullable: false),
                    rundeck_destroy_url = table.Column<string>(type: "VARCHAR(100)", nullable: true),
                    rundeck_create_url = table.Column<string>(type: "VARCHAR(100)", nullable: true),
                    rundeck_start_url = table.Column<string>(type: "VARCHAR(100)", nullable: true),
                    rundeck_stop_url = table.Column<string>(type: "VARCHAR(100)", nullable: true),
                    rundeck_snapshot_url = table.Column<string>(type: "VARCHAR(45)", nullable: true),
                    rundeck_restore_snapshot_url = table.Column<string>(type: "VARCHAR(45)", nullable: true),
                    estimated_cpus_used = table.Column<int>(nullable: false),
                    estimated_memory_used_mb = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_labs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    first_name = table.Column<string>(type: "VARCHAR(45)", nullable: false),
                    middle_name = table.Column<string>(type: "VARCHAR(45)", nullable: true),
                    last_name = table.Column<string>(type: "VARCHAR(45)", nullable: false),
                    school_email = table.Column<string>(type: "VARCHAR(45)", nullable: false),
                    personal_email = table.Column<string>(type: "VARCHAR(45)", nullable: false),
                    graduation_year = table.Column<string>(type: "VARCHAR(45)", nullable: false),
                    user_type = table.Column<string>(type: "VARCHAR(45)", nullable: false),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "UTC_TIMESTAMP()"),
                    updated_at = table.Column<DateTime>(nullable: false),
                    card_code_hash = table.Column<string>(type: "VARCHAR(45)", nullable: true),
                    termination_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_labs_name",
                table: "labs",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_users_card_code_hash",
                table: "users",
                column: "card_code_hash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_users_personal_email",
                table: "users",
                column: "personal_email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_users_school_email",
                table: "users",
                column: "school_email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "labs");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}

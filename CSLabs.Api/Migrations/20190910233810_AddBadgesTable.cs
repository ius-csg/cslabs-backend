using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CSLabs.Api.Migrations
{
    public partial class AddBadgesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "badges",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(nullable: false),
                    short_name = table.Column<string>(nullable: false),
                    description = table.Column<string>(type: "TEXT", nullable: false),
                    icon_path = table.Column<string>(type: "VARCHAR(255)", nullable: false),
                    required_num_of_modules = table.Column<int>(nullable: false),
                    created_at = table.Column<DateTime>(nullable: false, defaultValueSql: "UTC_TIMESTAMP()"),
                    updated_at = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_badges", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_badges_name",
                table: "badges",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_badges_short_name",
                table: "badges",
                column: "short_name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "badges");
        }
    }
}

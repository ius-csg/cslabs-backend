using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CSLabs.Api.Migrations
{
    public partial class AddTimeStamps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                table: "users",
                nullable: false,
                defaultValueSql: "UTC_TIMESTAMP()",
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                table: "modules",
                nullable: false,
                defaultValueSql: "UTC_TIMESTAMP()",
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                table: "labs",
                nullable: false,
                defaultValueSql: "UTC_TIMESTAMP()",
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                table: "badges",
                nullable: false,
                defaultValueSql: "UTC_TIMESTAMP()",
                oldClrType: typeof(DateTime));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                table: "users",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldDefaultValueSql: "UTC_TIMESTAMP()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                table: "modules",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldDefaultValueSql: "UTC_TIMESTAMP()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                table: "labs",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldDefaultValueSql: "UTC_TIMESTAMP()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                table: "badges",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldDefaultValueSql: "UTC_TIMESTAMP()");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CSLabs.Api.Migrations
{
    public partial class AddEndDateTimeToUserLab : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "end_date_time",
                table: "user_labs",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_labs_end_date_time",
                table: "user_labs",
                column: "end_date_time");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_user_labs_end_date_time",
                table: "user_labs");

            migrationBuilder.DropColumn(
                name: "end_date_time",
                table: "user_labs");
        }
    }
}

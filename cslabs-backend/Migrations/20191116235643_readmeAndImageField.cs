using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CSLabsBackend.Migrations
{
    public partial class readmeAndImageField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "image",
                table: "user_labs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "read_me",
                table: "user_labs",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "image",
                table: "labs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "read_me",
                table: "labs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "image",
                table: "user_labs");

            migrationBuilder.DropColumn(
                name: "read_me",
                table: "user_labs");

            migrationBuilder.DropColumn(
                name: "image",
                table: "labs");

            migrationBuilder.DropColumn(
                name: "read_me",
                table: "labs");
        }
    }
}

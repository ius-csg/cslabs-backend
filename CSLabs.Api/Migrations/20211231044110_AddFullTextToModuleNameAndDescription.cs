using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSLabs.Api.Migrations
{
    public partial class AddFullTextToModuleNameAndDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                ALTER TABLE modules ADD FULLTEXT(name)
            ");
            
            migrationBuilder.Sql(@"
                ALTER TABLE modules ADD FULLTEXT(description)
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

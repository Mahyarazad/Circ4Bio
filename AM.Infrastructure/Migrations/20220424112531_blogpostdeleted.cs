using Microsoft.EntityFrameworkCore.Migrations;

namespace AM.Infrastructure.Migrations
{
    public partial class blogpostdeleted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "dbo",
                table: "Blog",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "dbo",
                table: "Blog");
        }
    }
}

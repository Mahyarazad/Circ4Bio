using Microsoft.EntityFrameworkCore.Migrations;

namespace AM.Infrastructure.Migrations
{
    public partial class blogavatar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AvatarImage",
                schema: "dbo",
                table: "Blog",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                schema: "dbo",
                table: "Blog",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvatarImage",
                schema: "dbo",
                table: "Blog");

            migrationBuilder.DropColumn(
                name: "Slug",
                schema: "dbo",
                table: "Blog");
        }
    }
}

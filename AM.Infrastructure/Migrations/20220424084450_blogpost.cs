using Microsoft.EntityFrameworkCore.Migrations;

namespace AM.Infrastructure.Migrations
{
    public partial class blogpost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.AddColumn<string>(
                name: "Auther",
                schema: "dbo",
                table: "Blog",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                schema: "dbo",
                table: "Blog",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ReadDuration",
                schema: "dbo",
                table: "Blog",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Auther",
                schema: "dbo",
                table: "Blog");

            migrationBuilder.DropColumn(
                name: "Category",
                schema: "dbo",
                table: "Blog");

            migrationBuilder.DropColumn(
                name: "ReadDuration",
                schema: "dbo",
                table: "Blog");


        }
    }
}

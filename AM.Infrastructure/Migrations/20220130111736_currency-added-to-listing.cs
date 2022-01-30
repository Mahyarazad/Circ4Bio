using Microsoft.EntityFrameworkCore.Migrations;

namespace AM.Infrastructure.Migrations
{
    public partial class currencyaddedtolisting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                schema: "dbo",
                table: "Listing",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Currency",
                schema: "dbo",
                table: "Listing");

            migrationBuilder.RenameColumn(
                name: "Status",
                schema: "dbo",
                table: "Users",
                newName: "PublicStatus");

            migrationBuilder.RenameColumn(
                name: "Status",
                schema: "dbo",
                table: "Listing",
                newName: "PublicStatus");
        }
    }
}

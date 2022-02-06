using Microsoft.EntityFrameworkCore.Migrations;

namespace AM.Infrastructure.Migrations
{
    public partial class dealsupdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Currency",
                schema: "dbo",
                table: "Deals",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeliveryMethod",
                schema: "dbo",
                table: "Deals",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                schema: "dbo",
                table: "Deals",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                schema: "dbo",
                table: "Deals",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                schema: "dbo",
                table: "Deals",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Currency",
                schema: "dbo",
                table: "Deals");

            migrationBuilder.DropColumn(
                name: "DeliveryMethod",
                schema: "dbo",
                table: "Deals");

            migrationBuilder.DropColumn(
                name: "Latitude",
                schema: "dbo",
                table: "Deals");

            migrationBuilder.DropColumn(
                name: "Longitude",
                schema: "dbo",
                table: "Deals");

            migrationBuilder.DropColumn(
                name: "Unit",
                schema: "dbo",
                table: "Deals");
        }
    }
}

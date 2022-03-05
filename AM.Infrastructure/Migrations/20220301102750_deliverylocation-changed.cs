using Microsoft.EntityFrameworkCore.Migrations;

namespace AM.Infrastructure.Migrations
{
    public partial class deliverylocationchanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "DeliveryLocation");

            migrationBuilder.AddColumn<string>(
                name: "AddressLineOne",
                table: "DeliveryLocation",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AddressLineTwo",
                table: "DeliveryLocation",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "DeliveryLocation",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "DeliveryLocation",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "DeliveryLocation",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "DeliveryLocation",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "DeliveryLocation",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "PostalCode",
                table: "DeliveryLocation",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddressLineOne",
                table: "DeliveryLocation");

            migrationBuilder.DropColumn(
                name: "AddressLineTwo",
                table: "DeliveryLocation");

            migrationBuilder.DropColumn(
                name: "City",
                table: "DeliveryLocation");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "DeliveryLocation");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "DeliveryLocation");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "DeliveryLocation");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "DeliveryLocation");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "DeliveryLocation");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "DeliveryLocation",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }
    }
}

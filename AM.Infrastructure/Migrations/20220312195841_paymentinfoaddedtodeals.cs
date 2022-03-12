using Microsoft.EntityFrameworkCore.Migrations;

namespace AM.Infrastructure.Migrations
{
    public partial class paymentinfoaddedtodeals : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PayerEmail",
                schema: "dbo",
                table: "Deals",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PayerFirstName",
                schema: "dbo",
                table: "Deals",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PayerLastName",
                schema: "dbo",
                table: "Deals",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentDate",
                schema: "dbo",
                table: "Deals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentId",
                schema: "dbo",
                table: "Deals",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentTime",
                schema: "dbo",
                table: "Deals",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PayerEmail",
                schema: "dbo",
                table: "Deals");

            migrationBuilder.DropColumn(
                name: "PayerFirstName",
                schema: "dbo",
                table: "Deals");

            migrationBuilder.DropColumn(
                name: "PayerLastName",
                schema: "dbo",
                table: "Deals");

            migrationBuilder.DropColumn(
                name: "PaymentDate",
                schema: "dbo",
                table: "Deals");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                schema: "dbo",
                table: "Deals");

            migrationBuilder.DropColumn(
                name: "PaymentTime",
                schema: "dbo",
                table: "Deals");
        }
    }
}

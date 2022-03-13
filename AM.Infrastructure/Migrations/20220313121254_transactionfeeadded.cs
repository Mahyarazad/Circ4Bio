using Microsoft.EntityFrameworkCore.Migrations;

namespace AM.Infrastructure.Migrations
{
    public partial class transactionfeeadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "PaidAmount",
                schema: "dbo",
                table: "Deals",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TransactionFee",
                schema: "dbo",
                table: "Deals",
                type: "float",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaidAmount",
                schema: "dbo",
                table: "Deals");

            migrationBuilder.DropColumn(
                name: "TransactionFee",
                schema: "dbo",
                table: "Deals");
        }
    }
}

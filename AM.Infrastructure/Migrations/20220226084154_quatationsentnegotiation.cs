using Microsoft.EntityFrameworkCore.Migrations;

namespace AM.Infrastructure.Migrations
{
    public partial class Quotationsentnegotiation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRejected",
                schema: "dbo",
                table: "Negotiate",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "QuotationSent",
                schema: "dbo",
                table: "Negotiate",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRejected",
                schema: "dbo",
                table: "Negotiate");

            migrationBuilder.DropColumn(
                name: "QuotationSent",
                schema: "dbo",
                table: "Negotiate");
        }
    }
}

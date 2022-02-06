using Microsoft.EntityFrameworkCore.Migrations;

namespace AM.Infrastructure.Migrations
{
    public partial class dealsupdated3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "BuyerId",
                schema: "dbo",
                table: "Deals",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "SellerId",
                schema: "dbo",
                table: "Deals",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuyerId",
                schema: "dbo",
                table: "Deals");

            migrationBuilder.DropColumn(
                name: "SellerId",
                schema: "dbo",
                table: "Deals");
        }
    }
}

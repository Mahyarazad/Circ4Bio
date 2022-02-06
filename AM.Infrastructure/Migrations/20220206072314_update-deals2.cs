using Microsoft.EntityFrameworkCore.Migrations;

namespace AM.Infrastructure.Migrations
{
    public partial class updatedeals2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Cost",
                schema: "dbo",
                table: "Deals",
                newName: "TotalCost");

            migrationBuilder.AddColumn<long>(
                name: "DealId",
                schema: "dbo",
                table: "Negotiate",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DeliveryCost",
                schema: "dbo",
                table: "Deals",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<long>(
                name: "NegotiateId",
                schema: "dbo",
                table: "Deals",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Negotiate_DealId",
                schema: "dbo",
                table: "Negotiate",
                column: "DealId");

            migrationBuilder.AddForeignKey(
                name: "FK_Negotiate_Deals_DealId",
                schema: "dbo",
                table: "Negotiate",
                column: "DealId",
                principalSchema: "dbo",
                principalTable: "Deals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Negotiate_Deals_DealId",
                schema: "dbo",
                table: "Negotiate");

            migrationBuilder.DropIndex(
                name: "IX_Negotiate_DealId",
                schema: "dbo",
                table: "Negotiate");

            migrationBuilder.DropColumn(
                name: "DealId",
                schema: "dbo",
                table: "Negotiate");

            migrationBuilder.DropColumn(
                name: "DeliveryCost",
                schema: "dbo",
                table: "Deals");

            migrationBuilder.DropColumn(
                name: "NegotiateId",
                schema: "dbo",
                table: "Deals");

            migrationBuilder.RenameColumn(
                name: "TotalCost",
                schema: "dbo",
                table: "Deals",
                newName: "Cost");
        }
    }
}

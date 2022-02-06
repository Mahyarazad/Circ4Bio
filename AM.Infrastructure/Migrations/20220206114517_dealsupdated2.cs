using Microsoft.EntityFrameworkCore.Migrations;

namespace AM.Infrastructure.Migrations
{
    public partial class dealsupdated2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Deals_DealId",
                schema: "dbo",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_DealId",
                schema: "dbo",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DealId",
                schema: "dbo",
                table: "Users");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "DealId",
                schema: "dbo",
                table: "Users",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_DealId",
                schema: "dbo",
                table: "Users",
                column: "DealId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Deals_DealId",
                schema: "dbo",
                table: "Users",
                column: "DealId",
                principalSchema: "dbo",
                principalTable: "Deals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace AM.Infrastructure.Migrations
{
    public partial class usernegotiatio : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "UserId",
                schema: "dbo",
                table: "Negotiate",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Negotiate_UserId",
                schema: "dbo",
                table: "Negotiate",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Negotiate_Users_UserId",
                schema: "dbo",
                table: "Negotiate",
                column: "UserId",
                principalSchema: "dbo",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Negotiate_Users_UserId",
                schema: "dbo",
                table: "Negotiate");

            migrationBuilder.DropIndex(
                name: "IX_Negotiate_UserId",
                schema: "dbo",
                table: "Negotiate");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "dbo",
                table: "Negotiate");
        }
    }
}

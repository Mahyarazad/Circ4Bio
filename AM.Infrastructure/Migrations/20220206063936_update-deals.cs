using Microsoft.EntityFrameworkCore.Migrations;

namespace AM.Infrastructure.Migrations
{
    public partial class updatedeals : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClosingStatus",
                schema: "dbo",
                table: "Deals");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "dbo",
                table: "Deals",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsFinished",
                schema: "dbo",
                table: "Deals",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "dbo",
                table: "Deals");

            migrationBuilder.DropColumn(
                name: "IsFinished",
                schema: "dbo",
                table: "Deals");

            migrationBuilder.AddColumn<int>(
                name: "ClosingStatus",
                schema: "dbo",
                table: "Deals",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace AM.Infrastructure.Migrations
{
    public partial class nacedataadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                schema: "dbo",
                table: "NaceDataDetail",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "NaceId",
                schema: "dbo",
                table: "NaceData",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemId",
                schema: "dbo",
                table: "NaceDataDetail");

            migrationBuilder.DropColumn(
                name: "NaceId",
                schema: "dbo",
                table: "NaceData");
        }
    }
}

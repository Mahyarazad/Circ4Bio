using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AM.Infrastructure.Migrations
{
    public partial class listingtableupdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasAmount",
                schema: "dbo",
                table: "Listing",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsService",
                schema: "dbo",
                table: "Listing",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ListingOperation",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OperationType = table.Column<bool>(type: "bit", nullable: false),
                    ListingId = table.Column<long>(type: "bigint", nullable: false),
                    CurrentAmount = table.Column<double>(type: "float", nullable: false),
                    OperationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Count = table.Column<double>(type: "float", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    DealId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListingOperation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ListingOperation_Listing_ListingId",
                        column: x => x.ListingId,
                        principalSchema: "dbo",
                        principalTable: "Listing",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ListingOperation_ListingId",
                schema: "dbo",
                table: "ListingOperation",
                column: "ListingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ListingOperation",
                schema: "dbo");

            migrationBuilder.DropColumn(
                name: "HasAmount",
                schema: "dbo",
                table: "Listing");

            migrationBuilder.DropColumn(
                name: "IsService",
                schema: "dbo",
                table: "Listing");
        }
    }
}

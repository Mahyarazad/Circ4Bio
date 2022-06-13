using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AM.Infrastructure.Migrations
{
    public partial class nacedata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NaceData",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ListingId = table.Column<long>(type: "bigint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NaceData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NaceData_Listing_ListingId",
                        column: x => x.ListingId,
                        principalSchema: "dbo",
                        principalTable: "Listing",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NaceDataDetail",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NaceData = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NaceDataId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NaceDataDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NaceDataDetail_NaceData_NaceDataId",
                        column: x => x.NaceDataId,
                        principalSchema: "dbo",
                        principalTable: "NaceData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NaceData_ListingId",
                schema: "dbo",
                table: "NaceData",
                column: "ListingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NaceDataDetail_NaceDataId",
                schema: "dbo",
                table: "NaceDataDetail",
                column: "NaceDataId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NaceDataDetail",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "NaceData",
                schema: "dbo");
        }
    }
}

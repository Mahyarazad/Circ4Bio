using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AM.Infrastructure.Migrations
{
    public partial class NaceAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Nace",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ListingId = table.Column<long>(type: "bigint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nace", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Nace_Listing_ListingId",
                        column: x => x.ListingId,
                        principalSchema: "dbo",
                        principalTable: "Listing",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NaceItems",
                columns: table => new
                {
                    NaceDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DetailBody = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    NaceId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NaceItems", x => x.NaceDetailId);
                    table.ForeignKey(
                        name: "FK_NaceItems_Nace_NaceId",
                        column: x => x.NaceId,
                        principalSchema: "dbo",
                        principalTable: "Nace",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ListItemsDetail",
                columns: table => new
                {
                    ListItemDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ListItemDetail = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DetailNaceDetailId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListItemsDetail", x => x.ListItemDetailId);
                    table.ForeignKey(
                        name: "FK_ListItemsDetail_NaceItems_DetailNaceDetailId",
                        column: x => x.DetailNaceDetailId,
                        principalTable: "NaceItems",
                        principalColumn: "NaceDetailId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ListItemsDetail_DetailNaceDetailId",
                table: "ListItemsDetail",
                column: "DetailNaceDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_Nace_ListingId",
                schema: "dbo",
                table: "Nace",
                column: "ListingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NaceItems_NaceId",
                table: "NaceItems",
                column: "NaceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ListItemsDetail");

            migrationBuilder.DropTable(
                name: "NaceItems");

            migrationBuilder.DropTable(
                name: "Nace",
                schema: "dbo");
        }
    }
}

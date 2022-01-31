using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AM.Infrastructure.Migrations
{
    public partial class databaseinit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");


            migrationBuilder.CreateTable(
                name: "Negotiate",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ListingId = table.Column<long>(type: "bigint", nullable: false),
                    BuyyerId = table.Column<long>(type: "bigint", nullable: false),
                    SellerId = table.Column<long>(type: "bigint", nullable: false),
                    IsFinished = table.Column<bool>(type: "bit", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Negotiate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Negotiate_Listing_ListingId",
                        column: x => x.ListingId,
                        principalSchema: "dbo",
                        principalTable: "Listing",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });


            migrationBuilder.CreateTable(
                name: "NegitiateMessages",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageBody = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    UserEntity = table.Column<bool>(type: "bit", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NegotiateId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NegitiateMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NegitiateMessages_Negotiate_NegotiateId",
                        column: x => x.NegotiateId,
                        principalSchema: "dbo",
                        principalTable: "Negotiate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
            migrationBuilder.CreateIndex(
                name: "IX_NegitiateMessages_NegotiateId",
                table: "NegitiateMessages",
                column: "NegotiateId");

            migrationBuilder.CreateIndex(
                name: "IX_Negotiate_ListingId",
                schema: "dbo",
                table: "Negotiate",
                column: "ListingId");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Listing_Users_UserId",
                schema: "dbo",
                table: "Listing");

            migrationBuilder.DropTable(
                name: "Blog",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ContactUs",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ListingOperation",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "NegitiateMessages");

            migrationBuilder.DropTable(
                name: "Recipient",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ResetPassword",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropTable(
                name: "Negotiate",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Notification",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Deals",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PurchasedItem",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Roles",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "SuppliedItem",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Listing",
                schema: "dbo");
        }
    }
}

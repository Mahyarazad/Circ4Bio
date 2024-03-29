﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace AM.Infrastructure.Migrations
{
    public partial class QuotationConfirmed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "QuotationConfirm",
                schema: "dbo",
                table: "Negotiate",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "Amount",
                schema: "dbo",
                table: "SuppliedItem",
                type: "float",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "Amount",
                schema: "dbo",
                table: "PurchasedItem",
                type: "float",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuotationConfirm",
                schema: "dbo",
                table: "Negotiate");
        }
    }
}

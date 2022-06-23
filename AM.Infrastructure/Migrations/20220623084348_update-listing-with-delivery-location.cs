﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace AM.Infrastructure.Migrations
{
    public partial class updatelistingwithdeliverylocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeliveryLocationId",
                schema: "dbo",
                table: "Listing",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveryLocationId",
                schema: "dbo",
                table: "Listing");
        }
    }
}

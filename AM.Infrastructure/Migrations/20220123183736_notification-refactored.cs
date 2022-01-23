using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AM.Infrastructure.Migrations
{
    public partial class notificationrefactored : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReed",
                schema: "dbo",
                table: "Notification");

            migrationBuilder.RenameTable(
                name: "Recipient",
                newName: "Recipient",
                newSchema: "dbo");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                schema: "dbo",
                table: "Recipient",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsReed",
                schema: "dbo",
                table: "Recipient",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationTime",
                schema: "dbo",
                table: "Recipient");

            migrationBuilder.DropColumn(
                name: "IsReed",
                schema: "dbo",
                table: "Recipient");

            migrationBuilder.RenameTable(
                name: "Recipient",
                schema: "dbo",
                newName: "Recipient");

            migrationBuilder.AddColumn<bool>(
                name: "IsReed",
                schema: "dbo",
                table: "Notification",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}

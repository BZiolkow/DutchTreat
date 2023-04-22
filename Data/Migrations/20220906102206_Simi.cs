using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DutchTreat.Migrations
{
    public partial class Simi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 1,
                column: "OrderDate",
                value: new DateTime(2022, 9, 6, 10, 22, 6, 127, DateTimeKind.Utc).AddTicks(5486));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 1,
                column: "OrderDate",
                value: new DateTime(2022, 9, 6, 9, 25, 56, 539, DateTimeKind.Utc).AddTicks(381));
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TravelHelperDb.Migrations
{
    public partial class CurrentTravelDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CurrentDate",
                table: "TravelSet",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentDate",
                table: "TravelSet");
        }
    }
}

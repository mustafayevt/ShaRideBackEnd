using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ShaRide.Application.Migrations
{
    public partial class RideStateChangeDateTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "RideStateChangeDatetime",
                schema: "Ride",
                table: "Rides",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RideStateChangeDatetime",
                schema: "Ride",
                table: "Rides");
        }
    }
}

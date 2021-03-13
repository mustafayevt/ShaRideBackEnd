using Microsoft.EntityFrameworkCore.Migrations;

namespace ShaRide.Application.Migrations
{
    public partial class CarSeatCompositionKeys1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                schema: "Ride",
                table: "CarSeatComposition");

            migrationBuilder.DropColumn(
                name: "IsRowActive",
                schema: "Ride",
                table: "CarSeatComposition");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                schema: "Ride",
                table: "CarSeatComposition",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsRowActive",
                schema: "Ride",
                table: "CarSeatComposition",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}

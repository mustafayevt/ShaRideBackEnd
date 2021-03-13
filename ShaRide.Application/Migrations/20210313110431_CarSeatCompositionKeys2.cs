using Microsoft.EntityFrameworkCore.Migrations;

namespace ShaRide.Application.Migrations
{
    public partial class CarSeatCompositionKeys2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                schema: "Ride",
                table: "CarSeatComposition",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsRowActive",
                schema: "Ride",
                table: "CarSeatComposition",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}

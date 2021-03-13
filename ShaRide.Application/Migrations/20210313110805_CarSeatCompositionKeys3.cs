using Microsoft.EntityFrameworkCore.Migrations;

namespace ShaRide.Application.Migrations
{
    public partial class CarSeatCompositionKeys3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RideCarSeatComposition_CarSeatComposition_CarSeatCompositio~",
                schema: "Ride",
                table: "RideCarSeatComposition");

            migrationBuilder.DropColumn(
                name: "CarSeatCompositionId",
                schema: "Ride",
                table: "RideCarSeatComposition");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "Ride",
                table: "CarSeatComposition");

            migrationBuilder.DropColumn(
                name: "IsRowActive",
                schema: "Ride",
                table: "CarSeatComposition");

            migrationBuilder.AlterColumn<int>(
                name: "CarSeatCompositionSeatId",
                schema: "Ride",
                table: "RideCarSeatComposition",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "CarSeatCompositionCarId",
                schema: "Ride",
                table: "RideCarSeatComposition",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_RideCarSeatComposition_CarSeatComposition_CarSeatCompositio~",
                schema: "Ride",
                table: "RideCarSeatComposition",
                columns: new[] { "CarSeatCompositionSeatId", "CarSeatCompositionCarId" },
                principalSchema: "Ride",
                principalTable: "CarSeatComposition",
                principalColumns: new[] { "SeatId", "CarId" },
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RideCarSeatComposition_CarSeatComposition_CarSeatCompositio~",
                schema: "Ride",
                table: "RideCarSeatComposition");

            migrationBuilder.AlterColumn<int>(
                name: "CarSeatCompositionSeatId",
                schema: "Ride",
                table: "RideCarSeatComposition",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CarSeatCompositionCarId",
                schema: "Ride",
                table: "RideCarSeatComposition",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CarSeatCompositionId",
                schema: "Ride",
                table: "RideCarSeatComposition",
                type: "integer",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.AddForeignKey(
                name: "FK_RideCarSeatComposition_CarSeatComposition_CarSeatCompositio~",
                schema: "Ride",
                table: "RideCarSeatComposition",
                columns: new[] { "CarSeatCompositionSeatId", "CarSeatCompositionCarId" },
                principalSchema: "Ride",
                principalTable: "CarSeatComposition",
                principalColumns: new[] { "SeatId", "CarId" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}

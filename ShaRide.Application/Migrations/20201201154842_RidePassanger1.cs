using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ShaRide.Application.Migrations
{
    public partial class RidePassanger1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Car_CarBrands_CarBrandId",
                table: "Car");

            migrationBuilder.DropForeignKey(
                name: "FK_Car_CarModels_CarModelId",
                table: "Car");

            migrationBuilder.DropColumn(
                name: "LocationPointType",
                table: "LocationPoints");

            migrationBuilder.AddColumn<string>(
                name: "Note",
                schema: "Ride",
                table: "Rides",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PricePerSeat",
                schema: "Ride",
                table: "Rides",
                type: "decimal(18,6)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "RideState",
                schema: "Ride",
                table: "Rides",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                schema: "Ride",
                table: "Rides",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "PassengerId",
                schema: "Ride",
                table: "RideCarSeatComposition",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SeatType",
                schema: "Ride",
                table: "CarSeatComposition",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "CarModelId",
                table: "Car",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CarBrandId",
                table: "Car",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "RideLocationPointComposition",
                columns: table => new
                {
                    RideId = table.Column<int>(nullable: false),
                    LocationPointId = table.Column<int>(nullable: false),
                    LocationPointType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RideLocationPointComposition", x => new { x.RideId, x.LocationPointId });
                    table.ForeignKey(
                        name: "FK_RideLocationPointComposition_LocationPoints_LocationPointId",
                        column: x => x.LocationPointId,
                        principalTable: "LocationPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RideLocationPointComposition_Rides_RideId",
                        column: x => x.RideId,
                        principalSchema: "Ride",
                        principalTable: "Rides",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RideCarSeatComposition_PassengerId",
                schema: "Ride",
                table: "RideCarSeatComposition",
                column: "PassengerId");

            migrationBuilder.CreateIndex(
                name: "IX_RideLocationPointComposition_LocationPointId",
                table: "RideLocationPointComposition",
                column: "LocationPointId");

            migrationBuilder.AddForeignKey(
                name: "FK_Car_CarBrands_CarBrandId",
                table: "Car",
                column: "CarBrandId",
                principalTable: "CarBrands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Car_CarModels_CarModelId",
                table: "Car",
                column: "CarModelId",
                principalTable: "CarModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RideCarSeatComposition_User_PassengerId",
                schema: "Ride",
                table: "RideCarSeatComposition",
                column: "PassengerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Car_CarBrands_CarBrandId",
                table: "Car");

            migrationBuilder.DropForeignKey(
                name: "FK_Car_CarModels_CarModelId",
                table: "Car");

            migrationBuilder.DropForeignKey(
                name: "FK_RideCarSeatComposition_User_PassengerId",
                schema: "Ride",
                table: "RideCarSeatComposition");

            migrationBuilder.DropTable(
                name: "RideLocationPointComposition");

            migrationBuilder.DropIndex(
                name: "IX_RideCarSeatComposition_PassengerId",
                schema: "Ride",
                table: "RideCarSeatComposition");

            migrationBuilder.DropColumn(
                name: "Note",
                schema: "Ride",
                table: "Rides");

            migrationBuilder.DropColumn(
                name: "PricePerSeat",
                schema: "Ride",
                table: "Rides");

            migrationBuilder.DropColumn(
                name: "RideState",
                schema: "Ride",
                table: "Rides");

            migrationBuilder.DropColumn(
                name: "StartDate",
                schema: "Ride",
                table: "Rides");

            migrationBuilder.DropColumn(
                name: "PassengerId",
                schema: "Ride",
                table: "RideCarSeatComposition");

            migrationBuilder.DropColumn(
                name: "SeatType",
                schema: "Ride",
                table: "CarSeatComposition");

            migrationBuilder.AddColumn<int>(
                name: "LocationPointType",
                table: "LocationPoints",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "CarModelId",
                table: "Car",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "CarBrandId",
                table: "Car",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Car_CarBrands_CarBrandId",
                table: "Car",
                column: "CarBrandId",
                principalTable: "CarBrands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Car_CarModels_CarModelId",
                table: "Car",
                column: "CarModelId",
                principalTable: "CarModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace ShaRide.Application.Migrations
{
    public partial class userBalance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RideCarSeatComposition_Rides_RideId",
                schema: "Ride",
                table: "RideCarSeatComposition");

            migrationBuilder.AlterColumn<int>(
                name: "RideId",
                schema: "Ride",
                table: "RideCarSeatComposition",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Balance",
                table: "User",
                type: "decimal(18,6)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddForeignKey(
                name: "FK_RideCarSeatComposition_Rides_RideId",
                schema: "Ride",
                table: "RideCarSeatComposition",
                column: "RideId",
                principalSchema: "Ride",
                principalTable: "Rides",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RideCarSeatComposition_Rides_RideId",
                schema: "Ride",
                table: "RideCarSeatComposition");

            migrationBuilder.DropColumn(
                name: "Balance",
                table: "User");

            migrationBuilder.AlterColumn<int>(
                name: "RideId",
                schema: "Ride",
                table: "RideCarSeatComposition",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_RideCarSeatComposition_Rides_RideId",
                schema: "Ride",
                table: "RideCarSeatComposition",
                column: "RideId",
                principalSchema: "Ride",
                principalTable: "Rides",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

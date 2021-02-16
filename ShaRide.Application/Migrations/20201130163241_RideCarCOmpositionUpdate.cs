using Microsoft.EntityFrameworkCore.Migrations;

namespace ShaRide.Application.Migrations
{
    public partial class RideCarCOmpositionUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RideId",
                schema: "Ride",
                table: "RideCarSeatComposition",
                nullable: true);

            migrationBuilder.AlterColumn<short>(
                name: "Rating",
                table: "User",
                nullable: false,
                defaultValue: (short)5,
                oldClrType: typeof(short),
                oldType: "smallint");

            migrationBuilder.CreateIndex(
                name: "IX_RideCarSeatComposition_RideId",
                schema: "Ride",
                table: "RideCarSeatComposition",
                column: "RideId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RideCarSeatComposition_Rides_RideId",
                schema: "Ride",
                table: "RideCarSeatComposition");

            migrationBuilder.DropIndex(
                name: "IX_RideCarSeatComposition_RideId",
                schema: "Ride",
                table: "RideCarSeatComposition");

            migrationBuilder.DropColumn(
                name: "RideId",
                schema: "Ride",
                table: "RideCarSeatComposition");

            migrationBuilder.AlterColumn<short>(
                name: "Rating",
                table: "User",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(short),
                oldDefaultValue: (short)5);
        }
    }
}

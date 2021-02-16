using Microsoft.EntityFrameworkCore.Migrations;

namespace ShaRide.Application.Migrations
{
    public partial class RideCarCOmpositionUpdate1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RideCarSeatComposition_RideCarComposition_RideCarCompositio~",
                schema: "Ride",
                table: "RideCarSeatComposition");

            migrationBuilder.DropTable(
                name: "RideCarComposition",
                schema: "Ride");

            migrationBuilder.DropIndex(
                name: "IX_RideCarSeatComposition_RideCarCompositionCarId_RideCarCompo~",
                schema: "Ride",
                table: "RideCarSeatComposition");

            migrationBuilder.DropColumn(
                name: "RideCarCompositionCarId",
                schema: "Ride",
                table: "RideCarSeatComposition");

            migrationBuilder.DropColumn(
                name: "RideCarCompositionRideId",
                schema: "Ride",
                table: "RideCarSeatComposition");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RideCarCompositionCarId",
                schema: "Ride",
                table: "RideCarSeatComposition",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RideCarCompositionRideId",
                schema: "Ride",
                table: "RideCarSeatComposition",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RideCarComposition",
                schema: "Ride",
                columns: table => new
                {
                    CarId = table.Column<int>(type: "integer", nullable: false),
                    RideId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RideCarComposition", x => new { x.CarId, x.RideId });
                    table.ForeignKey(
                        name: "FK_RideCarComposition_Car_CarId",
                        column: x => x.CarId,
                        principalTable: "Car",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RideCarComposition_Rides_RideId",
                        column: x => x.RideId,
                        principalSchema: "Ride",
                        principalTable: "Rides",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RideCarSeatComposition_RideCarCompositionCarId_RideCarCompo~",
                schema: "Ride",
                table: "RideCarSeatComposition",
                columns: new[] { "RideCarCompositionCarId", "RideCarCompositionRideId" });

            migrationBuilder.CreateIndex(
                name: "IX_RideCarComposition_RideId",
                schema: "Ride",
                table: "RideCarComposition",
                column: "RideId");

            migrationBuilder.AddForeignKey(
                name: "FK_RideCarSeatComposition_RideCarComposition_RideCarCompositio~",
                schema: "Ride",
                table: "RideCarSeatComposition",
                columns: new[] { "RideCarCompositionCarId", "RideCarCompositionRideId" },
                principalSchema: "Ride",
                principalTable: "RideCarComposition",
                principalColumns: new[] { "CarId", "RideId" },
                onDelete: ReferentialAction.Restrict);
        }
    }
}

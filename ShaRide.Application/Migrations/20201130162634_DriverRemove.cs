using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ShaRide.Application.Migrations
{
    public partial class DriverRemove : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RideCarSeatComposition_CarSeatComposition_CarSeatCompositio~",
                schema: "Ride",
                table: "RideCarSeatComposition");

            migrationBuilder.DropForeignKey(
                name: "FK_Rides_Drivers_DriverId",
                schema: "Ride",
                table: "Rides");

            migrationBuilder.DropTable(
                name: "Drivers",
                schema: "Ride");

            migrationBuilder.DropIndex(
                name: "IX_Rides_DriverId",
                schema: "Ride",
                table: "Rides");

            migrationBuilder.DropIndex(
                name: "IX_RideCarSeatComposition_CarSeatCompositionCarId_CarSeatCompo~",
                schema: "Ride",
                table: "RideCarSeatComposition");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CarSeatComposition",
                schema: "Ride",
                table: "CarSeatComposition");

            migrationBuilder.DropColumn(
                name: "CarSeatCompositionCarId",
                schema: "Ride",
                table: "RideCarSeatComposition");

            migrationBuilder.DropColumn(
                name: "CarSeatCompositionSeatId",
                schema: "Ride",
                table: "RideCarSeatComposition");

            migrationBuilder.AddColumn<int>(
                name: "CarSeatCompositionId",
                schema: "Ride",
                table: "RideCarSeatComposition",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                schema: "Ride",
                table: "CarSeatComposition",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<bool>(
                name: "IsRowActive",
                schema: "Ride",
                table: "CarSeatComposition",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<short>(
                name: "Rating",
                table: "User",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CarSeatComposition",
                schema: "Ride",
                table: "CarSeatComposition",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_RideCarSeatComposition_CarSeatCompositionId",
                schema: "Ride",
                table: "RideCarSeatComposition",
                column: "CarSeatCompositionId");

            migrationBuilder.CreateIndex(
                name: "IX_CarSeatComposition_CarId",
                schema: "Ride",
                table: "CarSeatComposition",
                column: "CarId");

            migrationBuilder.AddForeignKey(
                name: "FK_RideCarSeatComposition_CarSeatComposition_CarSeatCompositio~",
                schema: "Ride",
                table: "RideCarSeatComposition",
                column: "CarSeatCompositionId",
                principalSchema: "Ride",
                principalTable: "CarSeatComposition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RideCarSeatComposition_CarSeatComposition_CarSeatCompositio~",
                schema: "Ride",
                table: "RideCarSeatComposition");

            migrationBuilder.DropIndex(
                name: "IX_RideCarSeatComposition_CarSeatCompositionId",
                schema: "Ride",
                table: "RideCarSeatComposition");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CarSeatComposition",
                schema: "Ride",
                table: "CarSeatComposition");

            migrationBuilder.DropIndex(
                name: "IX_CarSeatComposition_CarId",
                schema: "Ride",
                table: "CarSeatComposition");

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

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "User");

            migrationBuilder.AddColumn<int>(
                name: "CarSeatCompositionCarId",
                schema: "Ride",
                table: "RideCarSeatComposition",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CarSeatCompositionSeatId",
                schema: "Ride",
                table: "RideCarSeatComposition",
                type: "integer",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CarSeatComposition",
                schema: "Ride",
                table: "CarSeatComposition",
                columns: new[] { "CarId", "SeatId" });

            migrationBuilder.CreateTable(
                name: "Drivers",
                schema: "Ride",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Rating = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drivers", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Drivers_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rides_DriverId",
                schema: "Ride",
                table: "Rides",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_RideCarSeatComposition_CarSeatCompositionCarId_CarSeatCompo~",
                schema: "Ride",
                table: "RideCarSeatComposition",
                columns: new[] { "CarSeatCompositionCarId", "CarSeatCompositionSeatId" });

            migrationBuilder.AddForeignKey(
                name: "FK_RideCarSeatComposition_CarSeatComposition_CarSeatCompositio~",
                schema: "Ride",
                table: "RideCarSeatComposition",
                columns: new[] { "CarSeatCompositionCarId", "CarSeatCompositionSeatId" },
                principalSchema: "Ride",
                principalTable: "CarSeatComposition",
                principalColumns: new[] { "CarId", "SeatId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rides_Drivers_DriverId",
                schema: "Ride",
                table: "Rides",
                column: "DriverId",
                principalSchema: "Ride",
                principalTable: "Drivers",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

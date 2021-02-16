using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ShaRide.Application.Migrations
{
    public partial class Ride : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Ride");

            migrationBuilder.RenameTable(
                name: "Rides",
                newName: "Rides",
                newSchema: "Ride");

            migrationBuilder.AddColumn<int>(
                name: "DriverId",
                schema: "Ride",
                table: "Rides",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Car",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsRowActive = table.Column<bool>(nullable: false),
                    CarBrandId = table.Column<int>(nullable: true),
                    CarModelId = table.Column<int>(nullable: true),
                    RegisterNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Car", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Car_CarBrands_CarBrandId",
                        column: x => x.CarBrandId,
                        principalTable: "CarBrands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Car_CarModels_CarModelId",
                        column: x => x.CarModelId,
                        principalTable: "CarModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Drivers",
                schema: "Ride",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    Rating = table.Column<short>(nullable: false)
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

            migrationBuilder.CreateTable(
                name: "CarImages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsRowActive = table.Column<bool>(nullable: false),
                    CarId = table.Column<int>(nullable: false),
                    ImagePath = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarImages_Car_CarId",
                        column: x => x.CarId,
                        principalTable: "Car",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CarSeatComposition",
                schema: "Ride",
                columns: table => new
                {
                    CarId = table.Column<int>(nullable: false),
                    SeatId = table.Column<int>(nullable: false),
                    SeatRotate = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarSeatComposition", x => new { x.CarId, x.SeatId });
                    table.ForeignKey(
                        name: "FK_CarSeatComposition_Car_CarId",
                        column: x => x.CarId,
                        principalTable: "Car",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CarSeatComposition_Seats_SeatId",
                        column: x => x.SeatId,
                        principalTable: "Seats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RideCarComposition",
                schema: "Ride",
                columns: table => new
                {
                    CarId = table.Column<int>(nullable: false),
                    RideId = table.Column<int>(nullable: false)
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

            migrationBuilder.CreateTable(
                name: "RideCarSeatComposition",
                schema: "Ride",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsRowActive = table.Column<bool>(nullable: false),
                    CarSeatCompositionCarId = table.Column<int>(nullable: true),
                    CarSeatCompositionSeatId = table.Column<int>(nullable: true),
                    SeatStatus = table.Column<int>(nullable: false),
                    RideCarCompositionCarId = table.Column<int>(nullable: true),
                    RideCarCompositionRideId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RideCarSeatComposition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RideCarSeatComposition_CarSeatComposition_CarSeatCompositio~",
                        columns: x => new { x.CarSeatCompositionCarId, x.CarSeatCompositionSeatId },
                        principalSchema: "Ride",
                        principalTable: "CarSeatComposition",
                        principalColumns: new[] { "CarId", "SeatId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RideCarSeatComposition_RideCarComposition_RideCarCompositio~",
                        columns: x => new { x.RideCarCompositionCarId, x.RideCarCompositionRideId },
                        principalSchema: "Ride",
                        principalTable: "RideCarComposition",
                        principalColumns: new[] { "CarId", "RideId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rides_DriverId",
                schema: "Ride",
                table: "Rides",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_Car_CarBrandId",
                table: "Car",
                column: "CarBrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Car_CarModelId",
                table: "Car",
                column: "CarModelId");

            migrationBuilder.CreateIndex(
                name: "IX_CarImages_CarId",
                table: "CarImages",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_CarSeatComposition_SeatId",
                schema: "Ride",
                table: "CarSeatComposition",
                column: "SeatId");

            migrationBuilder.CreateIndex(
                name: "IX_RideCarComposition_RideId",
                schema: "Ride",
                table: "RideCarComposition",
                column: "RideId");

            migrationBuilder.CreateIndex(
                name: "IX_RideCarSeatComposition_CarSeatCompositionCarId_CarSeatCompo~",
                schema: "Ride",
                table: "RideCarSeatComposition",
                columns: new[] { "CarSeatCompositionCarId", "CarSeatCompositionSeatId" });

            migrationBuilder.CreateIndex(
                name: "IX_RideCarSeatComposition_RideCarCompositionCarId_RideCarCompo~",
                schema: "Ride",
                table: "RideCarSeatComposition",
                columns: new[] { "RideCarCompositionCarId", "RideCarCompositionRideId" });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rides_Drivers_DriverId",
                schema: "Ride",
                table: "Rides");

            migrationBuilder.DropTable(
                name: "CarImages");

            migrationBuilder.DropTable(
                name: "Drivers",
                schema: "Ride");

            migrationBuilder.DropTable(
                name: "RideCarSeatComposition",
                schema: "Ride");

            migrationBuilder.DropTable(
                name: "CarSeatComposition",
                schema: "Ride");

            migrationBuilder.DropTable(
                name: "RideCarComposition",
                schema: "Ride");

            migrationBuilder.DropTable(
                name: "Car");

            migrationBuilder.DropIndex(
                name: "IX_Rides_DriverId",
                schema: "Ride",
                table: "Rides");

            migrationBuilder.DropColumn(
                name: "DriverId",
                schema: "Ride",
                table: "Rides");

            migrationBuilder.RenameTable(
                name: "Rides",
                schema: "Ride",
                newName: "Rides");
        }
    }
}

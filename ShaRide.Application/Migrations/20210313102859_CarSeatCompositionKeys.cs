using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ShaRide.Application.Migrations
{
    public partial class CarSeatCompositionKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "IX_CarSeatComposition_SeatId",
                schema: "Ride",
                table: "CarSeatComposition");

            migrationBuilder.AddColumn<int>(
                name: "CarSeatCompositionCarId",
                schema: "Ride",
                table: "RideCarSeatComposition",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CarSeatCompositionSeatId",
                schema: "Ride",
                table: "RideCarSeatComposition",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                schema: "Ride",
                table: "CarSeatComposition",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CarSeatComposition",
                schema: "Ride",
                table: "CarSeatComposition",
                columns: new[] { "SeatId", "CarId" });

            migrationBuilder.CreateIndex(
                name: "IX_RideCarSeatComposition_CarSeatCompositionSeatId_CarSeatComp~",
                schema: "Ride",
                table: "RideCarSeatComposition",
                columns: new[] { "CarSeatCompositionSeatId", "CarSeatCompositionCarId" });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RideCarSeatComposition_CarSeatComposition_CarSeatCompositio~",
                schema: "Ride",
                table: "RideCarSeatComposition");

            migrationBuilder.DropIndex(
                name: "IX_RideCarSeatComposition_CarSeatCompositionSeatId_CarSeatComp~",
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

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                schema: "Ride",
                table: "CarSeatComposition",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

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
                name: "IX_CarSeatComposition_SeatId",
                schema: "Ride",
                table: "CarSeatComposition",
                column: "SeatId");

            migrationBuilder.AddForeignKey(
                name: "FK_RideCarSeatComposition_CarSeatComposition_CarSeatCompositio~",
                schema: "Ride",
                table: "RideCarSeatComposition",
                column: "CarSeatCompositionId",
                principalSchema: "Ride",
                principalTable: "CarSeatComposition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

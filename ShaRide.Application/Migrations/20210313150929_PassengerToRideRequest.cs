using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ShaRide.Application.Migrations
{
    public partial class PassengerToRideRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PassengerToRideRequests",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsRowActive = table.Column<bool>(nullable: false),
                    RideCarSeatCompositionId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    RequestStatus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PassengerToRideRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PassengerToRideRequests_RideCarSeatComposition_RideCarSeatC~",
                        column: x => x.RideCarSeatCompositionId,
                        principalSchema: "Ride",
                        principalTable: "RideCarSeatComposition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PassengerToRideRequests_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PassengerToRideRequests_RideCarSeatCompositionId",
                table: "PassengerToRideRequests",
                column: "RideCarSeatCompositionId");

            migrationBuilder.CreateIndex(
                name: "IX_PassengerToRideRequests_UserId",
                table: "PassengerToRideRequests",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PassengerToRideRequests");
        }
    }
}

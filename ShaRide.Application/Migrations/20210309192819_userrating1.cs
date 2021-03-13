using Microsoft.EntityFrameworkCore.Migrations;

namespace ShaRide.Application.Migrations
{
    public partial class userrating1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RideId",
                table: "UserRatings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserRatings_RideId",
                table: "UserRatings",
                column: "RideId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRatings_Rides_RideId",
                table: "UserRatings",
                column: "RideId",
                principalSchema: "Ride",
                principalTable: "Rides",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRatings_Rides_RideId",
                table: "UserRatings");

            migrationBuilder.DropIndex(
                name: "IX_UserRatings_RideId",
                table: "UserRatings");

            migrationBuilder.DropColumn(
                name: "RideId",
                table: "UserRatings");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace ShaRide.Application.Migrations
{
    public partial class RideLocationPointComposition1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "RideLocationPointComposition",
                newName: "RideLocationPointComposition",
                newSchema: "Ride");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "RideLocationPointComposition",
                schema: "Ride",
                newName: "RideLocationPointComposition");
        }
    }
}

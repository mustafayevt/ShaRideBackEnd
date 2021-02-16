using Microsoft.EntityFrameworkCore.Migrations;

namespace ShaRide.Application.Migrations
{
    public partial class RestrictionRideCompositionSchemaToRide : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "RestrictionRideComposition",
                newName: "RestrictionRideComposition",
                newSchema: "Ride");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "RestrictionRideComposition",
                schema: "Ride",
                newName: "RestrictionRideComposition");
        }
    }
}

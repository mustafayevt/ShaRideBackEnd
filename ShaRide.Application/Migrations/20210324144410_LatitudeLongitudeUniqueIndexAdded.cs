using Microsoft.EntityFrameworkCore.Migrations;

namespace ShaRide.Application.Migrations
{
    public partial class LatitudeLongitudeUniqueIndexAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_LocationPoints_Latitude_Longitude",
                table: "LocationPoints",
                columns: new[] { "Latitude", "Longitude" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LocationPoints_Latitude_Longitude",
                table: "LocationPoints");
        }
    }
}

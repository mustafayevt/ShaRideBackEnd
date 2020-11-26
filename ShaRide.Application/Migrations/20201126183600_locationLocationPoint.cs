using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ShaRide.Application.Migrations
{
    public partial class locationLocationPoint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Locations_ParentId",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Locations_ParentId",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "LocationType",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Locations");

            migrationBuilder.CreateTable(
                name: "LocationPoints",
                columns: table => new
                {
                    LocationId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LocationId1 = table.Column<int>(nullable: true),
                    LocationPointType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationPoints", x => x.LocationId);
                    table.ForeignKey(
                        name: "FK_LocationPoints_Locations_LocationId1",
                        column: x => x.LocationId1,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LocationPoints_LocationId1",
                table: "LocationPoints",
                column: "LocationId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LocationPoints");

            migrationBuilder.AddColumn<int>(
                name: "LocationType",
                table: "Locations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "Locations",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Locations_ParentId",
                table: "Locations",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Locations_ParentId",
                table: "Locations",
                column: "ParentId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

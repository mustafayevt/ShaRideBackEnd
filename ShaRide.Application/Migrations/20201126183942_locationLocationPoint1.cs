using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ShaRide.Application.Migrations
{
    public partial class locationLocationPoint1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LocationPoints_Locations_LocationId1",
                table: "LocationPoints");

            migrationBuilder.DropIndex(
                name: "IX_LocationPoints_LocationId1",
                table: "LocationPoints");

            migrationBuilder.DropColumn(
                name: "LocationId1",
                table: "LocationPoints");

            migrationBuilder.AlterColumn<int>(
                name: "LocationId",
                table: "LocationPoints",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddForeignKey(
                name: "FK_LocationPoints_Locations_LocationId",
                table: "LocationPoints",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LocationPoints_Locations_LocationId",
                table: "LocationPoints");

            migrationBuilder.AlterColumn<int>(
                name: "LocationId",
                table: "LocationPoints",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "LocationId1",
                table: "LocationPoints",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LocationPoints_LocationId1",
                table: "LocationPoints",
                column: "LocationId1");

            migrationBuilder.AddForeignKey(
                name: "FK_LocationPoints_Locations_LocationId1",
                table: "LocationPoints",
                column: "LocationId1",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

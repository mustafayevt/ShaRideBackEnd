using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ShaRide.Application.Migrations
{
    public partial class locationLocationPoint3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_LocationPoints",
                table: "LocationPoints");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "LocationPoints",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<bool>(
                name: "IsRowActive",
                table: "LocationPoints",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_LocationPoints",
                table: "LocationPoints",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_LocationPoints_LocationId",
                table: "LocationPoints",
                column: "LocationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_LocationPoints",
                table: "LocationPoints");

            migrationBuilder.DropIndex(
                name: "IX_LocationPoints_LocationId",
                table: "LocationPoints");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "LocationPoints");

            migrationBuilder.DropColumn(
                name: "IsRowActive",
                table: "LocationPoints");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LocationPoints",
                table: "LocationPoints",
                column: "LocationId");
        }
    }
}

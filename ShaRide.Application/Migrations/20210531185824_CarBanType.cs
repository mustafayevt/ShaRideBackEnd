using Microsoft.EntityFrameworkCore.Migrations;

namespace ShaRide.Application.Migrations
{
    public partial class CarBanType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarModels_BanTypes_BanTypeId",
                table: "CarModels");

            migrationBuilder.DropIndex(
                name: "IX_CarModels_BanTypeId",
                table: "CarModels");

            migrationBuilder.DropColumn(
                name: "BanTypeId",
                table: "CarModels");

            migrationBuilder.AddColumn<int>(
                name: "BanTypeId",
                table: "Cars",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Cars_BanTypeId",
                table: "Cars",
                column: "BanTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_BanTypes_BanTypeId",
                table: "Cars",
                column: "BanTypeId",
                principalTable: "BanTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_BanTypes_BanTypeId",
                table: "Cars");

            migrationBuilder.DropIndex(
                name: "IX_Cars_BanTypeId",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "BanTypeId",
                table: "Cars");

            migrationBuilder.AddColumn<int>(
                name: "BanTypeId",
                table: "CarModels",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CarModels_BanTypeId",
                table: "CarModels",
                column: "BanTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_CarModels_BanTypes_BanTypeId",
                table: "CarModels",
                column: "BanTypeId",
                principalTable: "BanTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

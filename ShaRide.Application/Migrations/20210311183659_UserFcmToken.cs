using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ShaRide.Application.Migrations
{
    public partial class UserFcmToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RideCarSeatComposition_CarSeatComposition_CarSeatCompositio~",
                schema: "Ride",
                table: "RideCarSeatComposition");

            migrationBuilder.AlterColumn<int>(
                name: "CarSeatCompositionId",
                schema: "Ride",
                table: "RideCarSeatComposition",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "UserFcmTokens",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsRowActive = table.Column<bool>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    Token = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFcmTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserFcmTokens_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserFcmTokens_UserId",
                table: "UserFcmTokens",
                column: "UserId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RideCarSeatComposition_CarSeatComposition_CarSeatCompositio~",
                schema: "Ride",
                table: "RideCarSeatComposition");

            migrationBuilder.DropTable(
                name: "UserFcmTokens");

            migrationBuilder.AlterColumn<int>(
                name: "CarSeatCompositionId",
                schema: "Ride",
                table: "RideCarSeatComposition",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_RideCarSeatComposition_CarSeatComposition_CarSeatCompositio~",
                schema: "Ride",
                table: "RideCarSeatComposition",
                column: "CarSeatCompositionId",
                principalSchema: "Ride",
                principalTable: "CarSeatComposition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

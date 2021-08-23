using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ShaRide.Application.Migrations
{
    public partial class CarUserMapping : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RideFeedbacks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsRowActive = table.Column<bool>(nullable: false),
                    CreatedByUserId = table.Column<int>(nullable: false),
                    CreatedTimestamp = table.Column<DateTime>(nullable: false),
                    LastModifiedByUserId = table.Column<int>(nullable: true),
                    LastModifiedTimestamp = table.Column<DateTime>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    MessageType = table.Column<int>(nullable: false),
                    SenderType = table.Column<int>(nullable: false),
                    RideId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RideFeedbacks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RideFeedbacks_User_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RideFeedbacks_User_LastModifiedByUserId",
                        column: x => x.LastModifiedByUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RideFeedbacks_Rides_RideId",
                        column: x => x.RideId,
                        principalSchema: "Ride",
                        principalTable: "Rides",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CarUserComposition",
                schema: "Ride",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsRowActive = table.Column<bool>(nullable: false),
                    CarId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarUserComposition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarUserComposition_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CarUserComposition_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Attachments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(nullable: true),
                    Content = table.Column<byte[]>(type: "bytea", nullable: true),
                    RideFeedbackId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attachments_RideFeedbacks_RideFeedbackId",
                        column: x => x.RideFeedbackId,
                        principalTable: "RideFeedbacks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_RideFeedbackId",
                table: "Attachments",
                column: "RideFeedbackId");

            migrationBuilder.CreateIndex(
                name: "IX_RideFeedbacks_CreatedByUserId",
                table: "RideFeedbacks",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RideFeedbacks_LastModifiedByUserId",
                table: "RideFeedbacks",
                column: "LastModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RideFeedbacks_RideId",
                table: "RideFeedbacks",
                column: "RideId");

            migrationBuilder.CreateIndex(
                name: "IX_CarUserComposition_CarId",
                schema: "Ride",
                table: "CarUserComposition",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_CarUserComposition_UserId",
                schema: "Ride",
                table: "CarUserComposition",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attachments");

            migrationBuilder.DropTable(
                name: "CarUserComposition",
                schema: "Ride");

            migrationBuilder.DropTable(
                name: "RideFeedbacks");
        }
    }
}

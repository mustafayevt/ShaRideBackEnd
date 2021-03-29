using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ShaRide.Application.Migrations
{
    public partial class Message : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Messages",
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
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_User_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Messages_User_LastModifiedByUserId",
                        column: x => x.LastModifiedByUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Messages_Rides_RideId",
                        column: x => x.RideId,
                        principalSchema: "Ride",
                        principalTable: "Rides",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_Content",
                table: "Messages",
                column: "Content",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_CreatedByUserId",
                table: "Messages",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_LastModifiedByUserId",
                table: "Messages",
                column: "LastModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_MessageType",
                table: "Messages",
                column: "MessageType",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_RideId",
                table: "Messages",
                column: "RideId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderType",
                table: "Messages",
                column: "SenderType",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Messages");
        }
    }
}

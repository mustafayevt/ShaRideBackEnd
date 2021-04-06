using Microsoft.EntityFrameworkCore.Migrations;

namespace ShaRide.Application.Migrations
{
    public partial class MessageUniqueCOnstraintDeleted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Messages_Content",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_MessageType",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_RideId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_SenderType",
                table: "Messages");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_RideId",
                table: "Messages",
                column: "RideId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Messages_RideId",
                table: "Messages");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_Content",
                table: "Messages",
                column: "Content",
                unique: true);

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
    }
}

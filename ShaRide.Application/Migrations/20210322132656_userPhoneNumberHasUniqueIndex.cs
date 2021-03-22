using Microsoft.EntityFrameworkCore.Migrations;

namespace ShaRide.Application.Migrations
{
    public partial class userPhoneNumberHasUniqueIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserPhones_Number",
                table: "UserPhones",
                column: "Number",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserPhones_Number",
                table: "UserPhones");
        }
    }
}

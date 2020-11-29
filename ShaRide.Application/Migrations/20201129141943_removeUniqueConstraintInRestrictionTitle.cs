using Microsoft.EntityFrameworkCore.Migrations;

namespace ShaRide.Application.Migrations
{
    public partial class removeUniqueConstraintInRestrictionTitle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Restrictions_Title",
                table: "Restrictions");

            migrationBuilder.CreateIndex(
                name: "IX_Restrictions_Title",
                table: "Restrictions",
                column: "Title");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Restrictions_Title",
                table: "Restrictions");

            migrationBuilder.CreateIndex(
                name: "IX_Restrictions_Title",
                table: "Restrictions",
                column: "Title",
                unique: true);
        }
    }
}

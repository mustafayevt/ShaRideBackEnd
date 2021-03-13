using Microsoft.EntityFrameworkCore.Migrations;

namespace ShaRide.Application.Migrations
{
    public partial class userrating4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "User");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "Rating",
                table: "User",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);
        }
    }
}

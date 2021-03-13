using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ShaRide.Application.Migrations
{
    public partial class userrating : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "User");

            migrationBuilder.CreateTable(
                name: "UserRatings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsRowActive = table.Column<bool>(nullable: false),
                    SourceUserId = table.Column<int>(nullable: false),
                    DestinationUserId = table.Column<int>(nullable: false),
                    Value = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRatings_User_DestinationUserId",
                        column: x => x.DestinationUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRatings_User_SourceUserId",
                        column: x => x.SourceUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserRatings_DestinationUserId",
                table: "UserRatings",
                column: "DestinationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRatings_SourceUserId",
                table: "UserRatings",
                column: "SourceUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserRatings");

            migrationBuilder.AddColumn<short>(
                name: "Rating",
                table: "User",
                type: "smallint",
                nullable: false,
                defaultValue: (short)5);
        }
    }
}

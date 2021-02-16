using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ShaRide.Application.Migrations
{
    public partial class Images : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Img",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ImgExtension",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "CarImages");

            migrationBuilder.AddColumn<string>(
                name: "Extension",
                table: "CarImages",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "CarImages",
                type: "bytea",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserImage",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsRowActive = table.Column<bool>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    Image = table.Column<byte[]>(type: "bytea", nullable: true),
                    Extension = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserImage_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserImage_UserId",
                table: "UserImage",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserImage");

            migrationBuilder.DropColumn(
                name: "Extension",
                table: "CarImages");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "CarImages");

            migrationBuilder.AddColumn<byte[]>(
                name: "Img",
                table: "User",
                type: "bytea",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImgExtension",
                table: "User",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "CarImages",
                type: "text",
                nullable: true);
        }
    }
}

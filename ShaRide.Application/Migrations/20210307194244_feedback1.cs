using Microsoft.EntityFrameworkCore.Migrations;

namespace ShaRide.Application.Migrations
{
    public partial class feedback1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_User_LastModifiedByUserId",
                table: "Feedbacks");

            migrationBuilder.AlterColumn<int>(
                name: "LastModifiedByUserId",
                table: "Feedbacks",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_User_LastModifiedByUserId",
                table: "Feedbacks",
                column: "LastModifiedByUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_User_LastModifiedByUserId",
                table: "Feedbacks");

            migrationBuilder.AlterColumn<int>(
                name: "LastModifiedByUserId",
                table: "Feedbacks",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_User_LastModifiedByUserId",
                table: "Feedbacks",
                column: "LastModifiedByUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

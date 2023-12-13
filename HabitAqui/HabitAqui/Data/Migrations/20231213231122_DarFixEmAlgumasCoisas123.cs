using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitAqui.Data.Migrations
{
    public partial class DarFixEmAlgumasCoisas123 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Estado_AspNetUsers_ApplicationUserId",
                table: "Estado");

            migrationBuilder.DropIndex(
                name: "IX_Estado_ApplicationUserId",
                table: "Estado");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Estado");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Estado",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Estado_ApplicationUserId",
                table: "Estado",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Estado_AspNetUsers_ApplicationUserId",
                table: "Estado",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

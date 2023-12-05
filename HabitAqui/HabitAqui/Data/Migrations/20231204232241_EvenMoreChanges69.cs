using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitAqui.Data.Migrations
{
    public partial class EvenMoreChanges69 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Estado_AspNetUsers_ApplicationUserId1",
                table: "Estado");

            migrationBuilder.DropIndex(
                name: "IX_Estado_ApplicationUserId1",
                table: "Estado");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId1",
                table: "Estado");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "Estado",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Estado_AspNetUsers_ApplicationUserId",
                table: "Estado");

            migrationBuilder.DropIndex(
                name: "IX_Estado_ApplicationUserId",
                table: "Estado");

            migrationBuilder.AlterColumn<int>(
                name: "ApplicationUserId",
                table: "Estado",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId1",
                table: "Estado",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Estado_ApplicationUserId1",
                table: "Estado",
                column: "ApplicationUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Estado_AspNetUsers_ApplicationUserId1",
                table: "Estado",
                column: "ApplicationUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}

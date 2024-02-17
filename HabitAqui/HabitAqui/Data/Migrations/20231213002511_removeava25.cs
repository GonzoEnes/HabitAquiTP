using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitAqui.Data.Migrations
{
    public partial class removeava25 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Avaliacao_AspNetUsers_ApplicationUserId",
                table: "Avaliacao");

            migrationBuilder.DropForeignKey(
                name: "FK_Avaliacao_Habitacoes_HabitacaoId",
                table: "Avaliacao");

            migrationBuilder.DropIndex(
                name: "IX_Avaliacao_ApplicationUserId",
                table: "Avaliacao");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Avaliacao");

            migrationBuilder.DropColumn(
                name: "AvaliacaoNota",
                table: "Avaliacao");

            migrationBuilder.AlterColumn<int>(
                name: "HabitacaoId",
                table: "Avaliacao",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Nota",
                table: "Avaliacao",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Avaliacao_Habitacoes_HabitacaoId",
                table: "Avaliacao",
                column: "HabitacaoId",
                principalTable: "Habitacoes",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Avaliacao_Habitacoes_HabitacaoId",
                table: "Avaliacao");

            migrationBuilder.DropColumn(
                name: "Nota",
                table: "Avaliacao");

            migrationBuilder.AlterColumn<int>(
                name: "HabitacaoId",
                table: "Avaliacao",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Avaliacao",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "AvaliacaoNota",
                table: "Avaliacao",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_Avaliacao_ApplicationUserId",
                table: "Avaliacao",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Avaliacao_AspNetUsers_ApplicationUserId",
                table: "Avaliacao",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Avaliacao_Habitacoes_HabitacaoId",
                table: "Avaliacao",
                column: "HabitacaoId",
                principalTable: "Habitacoes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

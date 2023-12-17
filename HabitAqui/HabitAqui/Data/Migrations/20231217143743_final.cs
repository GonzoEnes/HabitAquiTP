using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitAqui.Data.Migrations
{
    public partial class final : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Avaliacao_Habitacoes_HabitacaoId",
                table: "Avaliacao");

            migrationBuilder.DropIndex(
                name: "IX_Avaliacao_HabitacaoId",
                table: "Avaliacao");

            migrationBuilder.DropColumn(
                name: "HabitacaoId",
                table: "Avaliacao");

            migrationBuilder.AddColumn<int>(
                name: "AvaliacaoId",
                table: "Arrendamentos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Arrendamentos_AvaliacaoId",
                table: "Arrendamentos",
                column: "AvaliacaoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Arrendamentos_Avaliacao_AvaliacaoId",
                table: "Arrendamentos",
                column: "AvaliacaoId",
                principalTable: "Avaliacao",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Arrendamentos_Avaliacao_AvaliacaoId",
                table: "Arrendamentos");

            migrationBuilder.DropIndex(
                name: "IX_Arrendamentos_AvaliacaoId",
                table: "Arrendamentos");

            migrationBuilder.DropColumn(
                name: "AvaliacaoId",
                table: "Arrendamentos");

            migrationBuilder.AddColumn<int>(
                name: "HabitacaoId",
                table: "Avaliacao",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Avaliacao_HabitacaoId",
                table: "Avaliacao",
                column: "HabitacaoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Avaliacao_Habitacoes_HabitacaoId",
                table: "Avaliacao",
                column: "HabitacaoId",
                principalTable: "Habitacoes",
                principalColumn: "Id");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitAqui.Data.Migrations
{
    public partial class TableFixes2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IdContrato",
                table: "Habitacoes",
                newName: "ContratoId");

            migrationBuilder.CreateIndex(
                name: "IX_Habitacoes_ContratoId",
                table: "Habitacoes",
                column: "ContratoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Habitacoes_Contratos_ContratoId",
                table: "Habitacoes",
                column: "ContratoId",
                principalTable: "Contratos",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Habitacoes_Contratos_ContratoId",
                table: "Habitacoes");

            migrationBuilder.DropIndex(
                name: "IX_Habitacoes_ContratoId",
                table: "Habitacoes");

            migrationBuilder.RenameColumn(
                name: "ContratoId",
                table: "Habitacoes",
                newName: "IdContrato");
        }
    }
}

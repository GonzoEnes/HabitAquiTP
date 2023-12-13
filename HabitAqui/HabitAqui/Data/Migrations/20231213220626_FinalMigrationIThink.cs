using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitAqui.Data.Migrations
{
    public partial class FinalMigrationIThink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Arrendamentos_Estado_EstadoId",
                table: "Arrendamentos");

            migrationBuilder.RenameColumn(
                name: "EstadoId",
                table: "Arrendamentos",
                newName: "EstadoRececaoId");

            migrationBuilder.RenameIndex(
                name: "IX_Arrendamentos_EstadoId",
                table: "Arrendamentos",
                newName: "IX_Arrendamentos_EstadoRececaoId");

            migrationBuilder.AddColumn<int>(
                name: "EstadoEntregaId",
                table: "Arrendamentos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Arrendamentos_EstadoEntregaId",
                table: "Arrendamentos",
                column: "EstadoEntregaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Arrendamentos_Estado_EstadoEntregaId",
                table: "Arrendamentos",
                column: "EstadoEntregaId",
                principalTable: "Estado",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Arrendamentos_Estado_EstadoRececaoId",
                table: "Arrendamentos",
                column: "EstadoRececaoId",
                principalTable: "Estado",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Arrendamentos_Estado_EstadoEntregaId",
                table: "Arrendamentos");

            migrationBuilder.DropForeignKey(
                name: "FK_Arrendamentos_Estado_EstadoRececaoId",
                table: "Arrendamentos");

            migrationBuilder.DropIndex(
                name: "IX_Arrendamentos_EstadoEntregaId",
                table: "Arrendamentos");

            migrationBuilder.DropColumn(
                name: "EstadoEntregaId",
                table: "Arrendamentos");

            migrationBuilder.RenameColumn(
                name: "EstadoRececaoId",
                table: "Arrendamentos",
                newName: "EstadoId");

            migrationBuilder.RenameIndex(
                name: "IX_Arrendamentos_EstadoRececaoId",
                table: "Arrendamentos",
                newName: "IX_Arrendamentos_EstadoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Arrendamentos_Estado_EstadoId",
                table: "Arrendamentos",
                column: "EstadoId",
                principalTable: "Estado",
                principalColumn: "Id");
        }
    }
}

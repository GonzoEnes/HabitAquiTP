using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitAqui.Data.Migrations
{
    public partial class ChangesDB1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoriaId",
                table: "Habitacoes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LocadorId",
                table: "Habitacoes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DiasPeriodoArrendamento",
                table: "Agendamentos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Habitacoes_CategoriaId",
                table: "Habitacoes",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Habitacoes_LocadorId",
                table: "Habitacoes",
                column: "LocadorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Habitacoes_Categorias_CategoriaId",
                table: "Habitacoes",
                column: "CategoriaId",
                principalTable: "Categorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Habitacoes_Locadores_LocadorId",
                table: "Habitacoes",
                column: "LocadorId",
                principalTable: "Locadores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Habitacoes_Categorias_CategoriaId",
                table: "Habitacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Habitacoes_Locadores_LocadorId",
                table: "Habitacoes");

            migrationBuilder.DropIndex(
                name: "IX_Habitacoes_CategoriaId",
                table: "Habitacoes");

            migrationBuilder.DropIndex(
                name: "IX_Habitacoes_LocadorId",
                table: "Habitacoes");

            migrationBuilder.DropColumn(
                name: "CategoriaId",
                table: "Habitacoes");

            migrationBuilder.DropColumn(
                name: "LocadorId",
                table: "Habitacoes");

            migrationBuilder.DropColumn(
                name: "DiasPeriodoArrendamento",
                table: "Agendamentos");
        }
    }
}

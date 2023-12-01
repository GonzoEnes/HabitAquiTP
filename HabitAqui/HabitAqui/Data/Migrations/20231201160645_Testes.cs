using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitAqui.Data.Migrations
{
    public partial class Testes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Habitacoes_Agendamentos_ArrendamentoId",
                table: "Habitacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Habitacoes_Contratos_ContratoId",
                table: "Habitacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Habitacoes_Estado_EstadoId",
                table: "Habitacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Habitacoes_Locadores_LocadorId",
                table: "Habitacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Habitacoes_TipoHabitacoes_TipoHabitacaoId",
                table: "Habitacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Habitacoes_Tipologia_TipologiaId",
                table: "Habitacoes");

            migrationBuilder.DropTable(
                name: "Tipologia");

            migrationBuilder.DropIndex(
                name: "IX_Habitacoes_ArrendamentoId",
                table: "Habitacoes");

            migrationBuilder.DropIndex(
                name: "IX_Habitacoes_ContratoId",
                table: "Habitacoes");

            migrationBuilder.DropIndex(
                name: "IX_Habitacoes_EstadoId",
                table: "Habitacoes");

            migrationBuilder.DropIndex(
                name: "IX_Habitacoes_LocadorId",
                table: "Habitacoes");

            migrationBuilder.DropIndex(
                name: "IX_Habitacoes_TipoHabitacaoId",
                table: "Habitacoes");

            migrationBuilder.DropIndex(
                name: "IX_Habitacoes_TipologiaId",
                table: "Habitacoes");

            migrationBuilder.DropColumn(
                name: "ArrendamentoId",
                table: "Habitacoes");

            migrationBuilder.DropColumn(
                name: "ContratoId",
                table: "Habitacoes");

            migrationBuilder.DropColumn(
                name: "EstadoId",
                table: "Habitacoes");

            migrationBuilder.DropColumn(
                name: "LocadorId",
                table: "Habitacoes");

            migrationBuilder.DropColumn(
                name: "TipoHabitacaoId",
                table: "Habitacoes");

            migrationBuilder.DropColumn(
                name: "TipologiaId",
                table: "Habitacoes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ArrendamentoId",
                table: "Habitacoes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ContratoId",
                table: "Habitacoes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EstadoId",
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
                name: "TipoHabitacaoId",
                table: "Habitacoes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TipologiaId",
                table: "Habitacoes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Tipologia",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tipologia", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Habitacoes_ArrendamentoId",
                table: "Habitacoes",
                column: "ArrendamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Habitacoes_ContratoId",
                table: "Habitacoes",
                column: "ContratoId");

            migrationBuilder.CreateIndex(
                name: "IX_Habitacoes_EstadoId",
                table: "Habitacoes",
                column: "EstadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Habitacoes_LocadorId",
                table: "Habitacoes",
                column: "LocadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Habitacoes_TipoHabitacaoId",
                table: "Habitacoes",
                column: "TipoHabitacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Habitacoes_TipologiaId",
                table: "Habitacoes",
                column: "TipologiaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Habitacoes_Agendamentos_ArrendamentoId",
                table: "Habitacoes",
                column: "ArrendamentoId",
                principalTable: "Agendamentos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Habitacoes_Contratos_ContratoId",
                table: "Habitacoes",
                column: "ContratoId",
                principalTable: "Contratos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Habitacoes_Estado_EstadoId",
                table: "Habitacoes",
                column: "EstadoId",
                principalTable: "Estado",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Habitacoes_Locadores_LocadorId",
                table: "Habitacoes",
                column: "LocadorId",
                principalTable: "Locadores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Habitacoes_TipoHabitacoes_TipoHabitacaoId",
                table: "Habitacoes",
                column: "TipoHabitacaoId",
                principalTable: "TipoHabitacoes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Habitacoes_Tipologia_TipologiaId",
                table: "Habitacoes",
                column: "TipologiaId",
                principalTable: "Tipologia",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

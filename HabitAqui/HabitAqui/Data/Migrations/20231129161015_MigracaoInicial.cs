using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitAqui.Data.Migrations
{
    public partial class MigracaoInicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Agendamentos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustoArrendamento = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agendamentos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contratos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataFim = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contratos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Estado",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdHabitacao = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estado", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Locadores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Avaliacao = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locadores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoHabitacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoHabitacoes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Habitacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdContrato = table.Column<int>(type: "int", nullable: false),
                    ContratoId = table.Column<int>(type: "int", nullable: false),
                    Disponivel = table.Column<bool>(type: "bit", nullable: false),
                    Localizacao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdArrendamento = table.Column<int>(type: "int", nullable: false),
                    ArrendamentoId = table.Column<int>(type: "int", nullable: false),
                    IdTipo = table.Column<int>(type: "int", nullable: false),
                    TipoHabitacaoId = table.Column<int>(type: "int", nullable: false),
                    IdLocador = table.Column<int>(type: "int", nullable: false),
                    LocadorId = table.Column<int>(type: "int", nullable: false),
                    Avaliacao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdEstado = table.Column<int>(type: "int", nullable: false),
                    EstadoId = table.Column<int>(type: "int", nullable: false),
                    Danos = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Observacoes = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Habitacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Habitacoes_Agendamentos_ArrendamentoId",
                        column: x => x.ArrendamentoId,
                        principalTable: "Agendamentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Habitacoes_Contratos_ContratoId",
                        column: x => x.ContratoId,
                        principalTable: "Contratos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Habitacoes_Estado_EstadoId",
                        column: x => x.EstadoId,
                        principalTable: "Estado",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Habitacoes_Locadores_LocadorId",
                        column: x => x.LocadorId,
                        principalTable: "Locadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Habitacoes_TipoHabitacoes_TipoHabitacaoId",
                        column: x => x.TipoHabitacaoId,
                        principalTable: "TipoHabitacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reservas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdCliente = table.Column<int>(type: "int", nullable: false),
                    IdHabitacao = table.Column<int>(type: "int", nullable: false),
                    HabitacaoId = table.Column<int>(type: "int", nullable: false),
                    IdContrato = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservas_Habitacoes_HabitacaoId",
                        column: x => x.HabitacaoId,
                        principalTable: "Habitacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                name: "IX_Reservas_HabitacaoId",
                table: "Reservas",
                column: "HabitacaoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "Reservas");

            migrationBuilder.DropTable(
                name: "Habitacoes");

            migrationBuilder.DropTable(
                name: "Agendamentos");

            migrationBuilder.DropTable(
                name: "Contratos");

            migrationBuilder.DropTable(
                name: "Estado");

            migrationBuilder.DropTable(
                name: "Locadores");

            migrationBuilder.DropTable(
                name: "TipoHabitacoes");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitAqui.Data.Migrations
{
    public partial class addAvaliacaoHabitacao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Habitacoes_Contratos_ContratoId",
                table: "Habitacoes");

            migrationBuilder.DropTable(
                name: "Contratos");

            migrationBuilder.DropIndex(
                name: "IX_Habitacoes_ContratoId",
                table: "Habitacoes");

            migrationBuilder.DropColumn(
                name: "Avaliacao",
                table: "Habitacoes");

            migrationBuilder.DropColumn(
                name: "ContratoId",
                table: "Habitacoes");

            migrationBuilder.AddColumn<decimal>(
                name: "MediaAvaliacoes",
                table: "Habitacoes",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "Avaliacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Avalicao = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HabitacaoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Avaliacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Avaliacoes_AspNetUsers_AplicationUserId",
                        column: x => x.AplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Avaliacoes_Habitacoes_HabitacaoId",
                        column: x => x.HabitacaoId,
                        principalTable: "Habitacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Avaliacoes_AplicationUserId",
                table: "Avaliacoes",
                column: "AplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Avaliacoes_HabitacaoId",
                table: "Avaliacoes",
                column: "HabitacaoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Avaliacoes");

            migrationBuilder.DropColumn(
                name: "MediaAvaliacoes",
                table: "Habitacoes");

            migrationBuilder.AddColumn<decimal>(
                name: "Avaliacao",
                table: "Habitacoes",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ContratoId",
                table: "Habitacoes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Contratos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataFim = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contratos", x => x.Id);
                });

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
    }
}

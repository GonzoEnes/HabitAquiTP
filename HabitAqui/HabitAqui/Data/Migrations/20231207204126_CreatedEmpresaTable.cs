using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitAqui.Data.Migrations
{
    public partial class CreatedEmpresaTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Habitacoes_Contratos_ContratoId",
                table: "Habitacoes");

            migrationBuilder.DropTable(
                name: "Contratos");

            migrationBuilder.RenameColumn(
                name: "ContratoId",
                table: "Habitacoes",
                newName: "EmpresaId");

            migrationBuilder.RenameIndex(
                name: "IX_Habitacoes_ContratoId",
                table: "Habitacoes",
                newName: "IX_Habitacoes_EmpresaId");

            migrationBuilder.CreateTable(
                name: "Empresa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Avaliacao = table.Column<int>(type: "int", nullable: false),
                    Disponivel = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empresa", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Funcionario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmpresaId = table.Column<int>(type: "int", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Funcionario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Funcionario_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Funcionario_Empresa_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Gestor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmpresaId = table.Column<int>(type: "int", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gestor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Gestor_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Gestor_Empresa_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Funcionario_ApplicationUserId",
                table: "Funcionario",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Funcionario_EmpresaId",
                table: "Funcionario",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_Gestor_ApplicationUserId",
                table: "Gestor",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Gestor_EmpresaId",
                table: "Gestor",
                column: "EmpresaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Habitacoes_Empresa_EmpresaId",
                table: "Habitacoes",
                column: "EmpresaId",
                principalTable: "Empresa",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Habitacoes_Empresa_EmpresaId",
                table: "Habitacoes");

            migrationBuilder.DropTable(
                name: "Funcionario");

            migrationBuilder.DropTable(
                name: "Gestor");

            migrationBuilder.DropTable(
                name: "Empresa");

            migrationBuilder.RenameColumn(
                name: "EmpresaId",
                table: "Habitacoes",
                newName: "ContratoId");

            migrationBuilder.RenameIndex(
                name: "IX_Habitacoes_EmpresaId",
                table: "Habitacoes",
                newName: "IX_Habitacoes_ContratoId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Habitacoes_Contratos_ContratoId",
                table: "Habitacoes",
                column: "ContratoId",
                principalTable: "Contratos",
                principalColumn: "Id");
        }
    }
}

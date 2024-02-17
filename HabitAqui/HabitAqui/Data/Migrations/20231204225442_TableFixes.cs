using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitAqui.Data.Migrations
{
    public partial class TableFixes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reservas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Agendamentos",
                table: "Agendamentos");

            migrationBuilder.DropColumn(
                name: "IdArrendamento",
                table: "Habitacoes");

            migrationBuilder.DropColumn(
                name: "IdCategoria",
                table: "Habitacoes");

            migrationBuilder.RenameTable(
                name: "Agendamentos",
                newName: "Arrendamentos");

            migrationBuilder.RenameColumn(
                name: "IdLocador",
                table: "Habitacoes",
                newName: "TipologiaId");

            migrationBuilder.RenameColumn(
                name: "IdEstado",
                table: "Habitacoes",
                newName: "ArrendamentoId");

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "Habitacoes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "IdContrato",
                table: "Habitacoes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Arrendamentos",
                table: "Arrendamentos",
                column: "Id");

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
                name: "IX_Habitacoes_TipologiaId",
                table: "Habitacoes",
                column: "TipologiaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Habitacoes_Tipologia_TipologiaId",
                table: "Habitacoes",
                column: "TipologiaId",
                principalTable: "Tipologia",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Habitacoes_Tipologia_TipologiaId",
                table: "Habitacoes");

            migrationBuilder.DropTable(
                name: "Tipologia");

            migrationBuilder.DropIndex(
                name: "IX_Habitacoes_TipologiaId",
                table: "Habitacoes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Arrendamentos",
                table: "Arrendamentos");

            migrationBuilder.RenameTable(
                name: "Arrendamentos",
                newName: "Agendamentos");

            migrationBuilder.RenameColumn(
                name: "TipologiaId",
                table: "Habitacoes",
                newName: "IdLocador");

            migrationBuilder.RenameColumn(
                name: "ArrendamentoId",
                table: "Habitacoes",
                newName: "IdEstado");

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "Habitacoes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IdContrato",
                table: "Habitacoes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdArrendamento",
                table: "Habitacoes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdCategoria",
                table: "Habitacoes",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Agendamentos",
                table: "Agendamentos",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Reservas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HabitacaoId = table.Column<int>(type: "int", nullable: false),
                    IdCliente = table.Column<int>(type: "int", nullable: false),
                    IdContrato = table.Column<int>(type: "int", nullable: false),
                    IdHabitacao = table.Column<int>(type: "int", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                name: "IX_Reservas_HabitacaoId",
                table: "Reservas",
                column: "HabitacaoId");
        }
    }
}

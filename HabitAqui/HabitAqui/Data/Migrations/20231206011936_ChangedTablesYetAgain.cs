using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitAqui.Data.Migrations
{
    public partial class ChangedTablesYetAgain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Habitacoes_Locadores_LocadorId",
                table: "Habitacoes");

            migrationBuilder.DropTable(
                name: "Locadores");

            migrationBuilder.DropIndex(
                name: "IX_Habitacoes_LocadorId",
                table: "Habitacoes");

            migrationBuilder.DropColumn(
                name: "ArrendamentoId",
                table: "Habitacoes");

            migrationBuilder.DropColumn(
                name: "LocadorId",
                table: "Habitacoes");

            migrationBuilder.AlterColumn<string>(
                name: "Avaliacao",
                table: "Habitacoes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "HabitacaoId",
                table: "Arrendamentos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Arrendamentos_HabitacaoId",
                table: "Arrendamentos",
                column: "HabitacaoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Arrendamentos_Habitacoes_HabitacaoId",
                table: "Arrendamentos",
                column: "HabitacaoId",
                principalTable: "Habitacoes",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Arrendamentos_Habitacoes_HabitacaoId",
                table: "Arrendamentos");

            migrationBuilder.DropIndex(
                name: "IX_Arrendamentos_HabitacaoId",
                table: "Arrendamentos");

            migrationBuilder.DropColumn(
                name: "HabitacaoId",
                table: "Arrendamentos");

            migrationBuilder.AlterColumn<string>(
                name: "Avaliacao",
                table: "Habitacoes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ArrendamentoId",
                table: "Habitacoes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LocadorId",
                table: "Habitacoes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Locadores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Avaliacao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locadores", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Habitacoes_LocadorId",
                table: "Habitacoes",
                column: "LocadorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Habitacoes_Locadores_LocadorId",
                table: "Habitacoes",
                column: "LocadorId",
                principalTable: "Locadores",
                principalColumn: "Id");
        }
    }
}

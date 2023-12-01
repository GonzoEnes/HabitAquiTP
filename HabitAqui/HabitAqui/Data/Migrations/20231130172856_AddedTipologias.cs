using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitAqui.Data.Migrations
{
    public partial class AddedTipologias : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdTipologia",
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
                name: "IX_Habitacoes_TipologiaId",
                table: "Habitacoes",
                column: "TipologiaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Habitacoes_Tipologia_TipologiaId",
                table: "Habitacoes",
                column: "TipologiaId",
                principalTable: "Tipologia",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.DropColumn(
                name: "IdTipologia",
                table: "Habitacoes");

            migrationBuilder.DropColumn(
                name: "TipologiaId",
                table: "Habitacoes");
        }
    }
}

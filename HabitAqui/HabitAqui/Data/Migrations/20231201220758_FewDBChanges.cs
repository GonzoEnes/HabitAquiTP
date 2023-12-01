using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitAqui.Data.Migrations
{
    public partial class FewDBChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdEstado",
                table: "Habitacoes");

            migrationBuilder.DropColumn(
                name: "IdTipo",
                table: "Habitacoes");

            migrationBuilder.RenameColumn(
                name: "IdTipologia",
                table: "Habitacoes",
                newName: "IdCategoria");

            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "Habitacoes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Tipo",
                table: "Habitacoes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Habitacoes");

            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "Habitacoes");

            migrationBuilder.RenameColumn(
                name: "IdCategoria",
                table: "Habitacoes",
                newName: "IdTipologia");

            migrationBuilder.AddColumn<int>(
                name: "IdEstado",
                table: "Habitacoes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdTipo",
                table: "Habitacoes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

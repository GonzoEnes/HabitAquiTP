using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitAqui.Data.Migrations
{
    public partial class RemovedIdHabitacaoFromEstadoTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdHabitacao",
                table: "Estado");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Categorias",
                newName: "Nome");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Nome",
                table: "Categorias",
                newName: "Name");

            migrationBuilder.AddColumn<int>(
                name: "IdHabitacao",
                table: "Estado",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

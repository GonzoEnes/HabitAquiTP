using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitAqui.Data.Migrations
{
    public partial class MigracaoImageHabitaaco : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Habitacoes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Habitacoes");
        }
    }
}

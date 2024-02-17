using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitAqui.Data.Migrations
{
    public partial class CorrigidoUmasCenas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "Habitacoes");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataPedido",
                table: "Arrendamentos",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataPedido",
                table: "Arrendamentos");

            migrationBuilder.AddColumn<string>(
                name: "Tipo",
                table: "Habitacoes",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

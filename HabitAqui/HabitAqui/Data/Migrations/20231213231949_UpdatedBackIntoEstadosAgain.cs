using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitAqui.Data.Migrations
{
    public partial class UpdatedBackIntoEstadosAgain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Arrendamentos_Estado_EstadoEntregaId",
                table: "Arrendamentos");

            migrationBuilder.DropForeignKey(
                name: "FK_Arrendamentos_Estado_EstadoRececaoId",
                table: "Arrendamentos");

            migrationBuilder.DropForeignKey(
                name: "FK_Habitacoes_Estado_EstadoId",
                table: "Habitacoes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Estado",
                table: "Estado");

            migrationBuilder.RenameTable(
                name: "Estado",
                newName: "Estados");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Estados",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Estados",
                table: "Estados",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Estados_ApplicationUserId",
                table: "Estados",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Arrendamentos_Estados_EstadoEntregaId",
                table: "Arrendamentos",
                column: "EstadoEntregaId",
                principalTable: "Estados",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Arrendamentos_Estados_EstadoRececaoId",
                table: "Arrendamentos",
                column: "EstadoRececaoId",
                principalTable: "Estados",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Estados_AspNetUsers_ApplicationUserId",
                table: "Estados",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Habitacoes_Estados_EstadoId",
                table: "Habitacoes",
                column: "EstadoId",
                principalTable: "Estados",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Arrendamentos_Estados_EstadoEntregaId",
                table: "Arrendamentos");

            migrationBuilder.DropForeignKey(
                name: "FK_Arrendamentos_Estados_EstadoRececaoId",
                table: "Arrendamentos");

            migrationBuilder.DropForeignKey(
                name: "FK_Estados_AspNetUsers_ApplicationUserId",
                table: "Estados");

            migrationBuilder.DropForeignKey(
                name: "FK_Habitacoes_Estados_EstadoId",
                table: "Habitacoes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Estados",
                table: "Estados");

            migrationBuilder.DropIndex(
                name: "IX_Estados_ApplicationUserId",
                table: "Estados");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Estados");

            migrationBuilder.RenameTable(
                name: "Estados",
                newName: "Estado");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Estado",
                table: "Estado",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Arrendamentos_Estado_EstadoEntregaId",
                table: "Arrendamentos",
                column: "EstadoEntregaId",
                principalTable: "Estado",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Arrendamentos_Estado_EstadoRececaoId",
                table: "Arrendamentos",
                column: "EstadoRececaoId",
                principalTable: "Estado",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Habitacoes_Estado_EstadoId",
                table: "Habitacoes",
                column: "EstadoId",
                principalTable: "Estado",
                principalColumn: "Id");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitAqui.Data.Migrations
{
    public partial class MudancasDoBarreiro : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Funcionario_AspNetUsers_ApplicationUserId",
                table: "Funcionario");

            migrationBuilder.DropForeignKey(
                name: "FK_Funcionario_Empresa_EmpresaId",
                table: "Funcionario");

            migrationBuilder.DropForeignKey(
                name: "FK_Gestor_AspNetUsers_ApplicationUserId",
                table: "Gestor");

            migrationBuilder.DropForeignKey(
                name: "FK_Gestor_Empresa_EmpresaId",
                table: "Gestor");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Gestor",
                table: "Gestor");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Funcionario",
                table: "Funcionario");

            migrationBuilder.RenameTable(
                name: "Gestor",
                newName: "Gestores");

            migrationBuilder.RenameTable(
                name: "Funcionario",
                newName: "Funcionarios");

            migrationBuilder.RenameIndex(
                name: "IX_Gestor_EmpresaId",
                table: "Gestores",
                newName: "IX_Gestores_EmpresaId");

            migrationBuilder.RenameIndex(
                name: "IX_Gestor_ApplicationUserId",
                table: "Gestores",
                newName: "IX_Gestores_ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Funcionario_EmpresaId",
                table: "Funcionarios",
                newName: "IX_Funcionarios_EmpresaId");

            migrationBuilder.RenameIndex(
                name: "IX_Funcionario_ApplicationUserId",
                table: "Funcionarios",
                newName: "IX_Funcionarios_ApplicationUserId");

            migrationBuilder.AlterColumn<bool>(
                name: "Disponivel",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "Gestores",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "Funcionarios",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Gestores",
                table: "Gestores",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Funcionarios",
                table: "Funcionarios",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Funcionarios_AspNetUsers_ApplicationUserId",
                table: "Funcionarios",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Funcionarios_Empresa_EmpresaId",
                table: "Funcionarios",
                column: "EmpresaId",
                principalTable: "Empresa",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Gestores_AspNetUsers_ApplicationUserId",
                table: "Gestores",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Gestores_Empresa_EmpresaId",
                table: "Gestores",
                column: "EmpresaId",
                principalTable: "Empresa",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Funcionarios_AspNetUsers_ApplicationUserId",
                table: "Funcionarios");

            migrationBuilder.DropForeignKey(
                name: "FK_Funcionarios_Empresa_EmpresaId",
                table: "Funcionarios");

            migrationBuilder.DropForeignKey(
                name: "FK_Gestores_AspNetUsers_ApplicationUserId",
                table: "Gestores");

            migrationBuilder.DropForeignKey(
                name: "FK_Gestores_Empresa_EmpresaId",
                table: "Gestores");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Gestores",
                table: "Gestores");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Funcionarios",
                table: "Funcionarios");

            migrationBuilder.RenameTable(
                name: "Gestores",
                newName: "Gestor");

            migrationBuilder.RenameTable(
                name: "Funcionarios",
                newName: "Funcionario");

            migrationBuilder.RenameIndex(
                name: "IX_Gestores_EmpresaId",
                table: "Gestor",
                newName: "IX_Gestor_EmpresaId");

            migrationBuilder.RenameIndex(
                name: "IX_Gestores_ApplicationUserId",
                table: "Gestor",
                newName: "IX_Gestor_ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Funcionarios_EmpresaId",
                table: "Funcionario",
                newName: "IX_Funcionario_EmpresaId");

            migrationBuilder.RenameIndex(
                name: "IX_Funcionarios_ApplicationUserId",
                table: "Funcionario",
                newName: "IX_Funcionario_ApplicationUserId");

            migrationBuilder.AlterColumn<bool>(
                name: "Disponivel",
                table: "AspNetUsers",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "Gestor",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "Funcionario",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Gestor",
                table: "Gestor",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Funcionario",
                table: "Funcionario",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Funcionario_AspNetUsers_ApplicationUserId",
                table: "Funcionario",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Funcionario_Empresa_EmpresaId",
                table: "Funcionario",
                column: "EmpresaId",
                principalTable: "Empresa",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Gestor_AspNetUsers_ApplicationUserId",
                table: "Gestor",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Gestor_Empresa_EmpresaId",
                table: "Gestor",
                column: "EmpresaId",
                principalTable: "Empresa",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

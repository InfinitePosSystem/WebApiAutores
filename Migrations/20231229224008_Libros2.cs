using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiAutores.Migrations
{
    public partial class Libros2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entidades_Autores_AutorId",
                table: "Entidades");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Entidades",
                table: "Entidades");

            migrationBuilder.RenameTable(
                name: "Entidades",
                newName: "Libro");

            migrationBuilder.RenameIndex(
                name: "IX_Entidades_AutorId",
                table: "Libro",
                newName: "IX_Libro_AutorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Libro",
                table: "Libro",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Libro_Autores_AutorId",
                table: "Libro",
                column: "AutorId",
                principalTable: "Autores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Libro_Autores_AutorId",
                table: "Libro");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Libro",
                table: "Libro");

            migrationBuilder.RenameTable(
                name: "Libro",
                newName: "Entidades");

            migrationBuilder.RenameIndex(
                name: "IX_Libro_AutorId",
                table: "Entidades",
                newName: "IX_Entidades_AutorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Entidades",
                table: "Entidades",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Entidades_Autores_AutorId",
                table: "Entidades",
                column: "AutorId",
                principalTable: "Autores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

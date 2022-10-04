using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JwtTeach.Migrations
{
    public partial class relacionUsuarioArticulo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UsuarioId",
                table: "Articulos",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Articulos_UsuarioId",
                table: "Articulos",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Articulos_AspNetUsers_UsuarioId",
                table: "Articulos",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articulos_AspNetUsers_UsuarioId",
                table: "Articulos");

            migrationBuilder.DropIndex(
                name: "IX_Articulos_UsuarioId",
                table: "Articulos");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Articulos");
        }
    }
}

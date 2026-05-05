using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiendaVirtualNarvaez.Migrations
{
    /// <inheritdoc />
    public partial class AddImagenProducto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // CAMBIA AlterColumn por AddColumn para la Clave
            migrationBuilder.AddColumn<string>(
                name: "Clave",
                table: "Usuarios",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            // Asegúrate de que la ImagenUrl también sea AddColumn
            migrationBuilder.AddColumn<string>(
                name: "ImagenUrl",
                table: "Productos",
                type: "nvarchar(max)",
                nullable: true); // La ponemos como opcional para evitar errores con datos existentes
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Clave",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "ImagenUrl",
                table: "Productos");
        }
    }
}

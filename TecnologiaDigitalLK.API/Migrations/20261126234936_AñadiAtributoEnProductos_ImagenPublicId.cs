using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TecnologiaDigitalLK.API.Migrations
{
    /// <inheritdoc />
    public partial class AñadiAtributoEnProductos_ImagenPublicId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagenPublicId",
                table: "Productos",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagenPublicId",
                table: "Productos");
        }
    }
}

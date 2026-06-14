using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoWebApi.Migrations
{
    /// <inheritdoc />
    public partial class RenameFieldMakeToManufacturer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Make",
                table: "Cars",
                newName: "Manufacturer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Manufacturer",
                table: "Cars",
                newName: "Make");
        }
    }
}

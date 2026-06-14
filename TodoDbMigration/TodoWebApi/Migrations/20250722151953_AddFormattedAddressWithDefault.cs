using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoWebApi.Migrations
{
    /// <inheritdoc />
    public partial class AddFormattedAddressWithDefault : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FormattedAddress",
                table: "CarOwners",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FormattedAddress",
                table: "CarOwners");
        }
    }
}

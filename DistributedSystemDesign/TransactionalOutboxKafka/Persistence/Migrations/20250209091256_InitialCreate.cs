using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransactionalOutboxKafka.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //Transaction control
            //To address concurrency, Uses the optimistic concurrency control
            migrationBuilder.Sql("ALTER DATABASE CURRENT SET ALLOW_SNAPSHOT_ISOLATION ON;", true);
            migrationBuilder.Sql("ALTER DATABASE CURRENT SET READ_COMMITTED_SNAPSHOT ON;", true);

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Invoices");
        }
    }
}

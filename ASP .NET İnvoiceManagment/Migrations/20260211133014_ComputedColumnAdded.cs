using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASP_.NET_InvoiceManagment.Migrations
{
    /// <inheritdoc />
    public partial class ComputedColumnAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Sum",
                table: "InvoiceRows",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                computedColumnSql: "[Quantity] * [Amount]",
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Sum",
                table: "InvoiceRows",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldComputedColumnSql: "[Quantity] * [Amount]");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancialSolution.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUniqueTransactionReference : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Transactions_Reference",
                table: "Transactions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Transactions_Reference",
                table: "Transactions",
                column: "Reference",
                unique: true);
        }
    }
}

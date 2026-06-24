using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancialSolution.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFailedAttemptCountToScheduledTransfer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FailedAttemptCount",
                table: "ScheduledTransfers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FailedAttemptCount",
                table: "ScheduledTransfers");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancialSolution.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBvnVerifiedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "BvnVerifiedAt",
                table: "Customers",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BvnVerifiedAt",
                table: "Customers");
        }
    }
}

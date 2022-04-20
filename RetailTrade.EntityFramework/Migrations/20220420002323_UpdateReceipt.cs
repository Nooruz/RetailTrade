using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetailTrade.EntityFramework.Migrations
{
    public partial class UpdateReceipt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Sum",
                table: "Receipts",
                newName: "Total");

            migrationBuilder.RenameColumn(
                name: "Deposited",
                table: "Receipts",
                newName: "DiscountAmount");

            migrationBuilder.AddColumn<decimal>(
                name: "AmountWithoutDiscount",
                table: "Receipts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountWithoutDiscount",
                table: "Receipts");

            migrationBuilder.RenameColumn(
                name: "Total",
                table: "Receipts",
                newName: "Sum");

            migrationBuilder.RenameColumn(
                name: "DiscountAmount",
                table: "Receipts",
                newName: "Deposited");
        }
    }
}

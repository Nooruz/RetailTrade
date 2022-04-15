using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetailTrade.EntityFramework.Migrations
{
    public partial class UpdateProductSale : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Sum",
                table: "ProductSales",
                newName: "Total");

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountAmount",
                table: "ProductSales",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Contractors",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscountAmount",
                table: "ProductSales");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Contractors");

            migrationBuilder.RenameColumn(
                name: "Total",
                table: "ProductSales",
                newName: "Sum");
        }
    }
}

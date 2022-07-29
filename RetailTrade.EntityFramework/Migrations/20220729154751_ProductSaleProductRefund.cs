using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetailTrade.EntityFramework.Migrations
{
    public partial class ProductSaleProductRefund : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRefund",
                table: "ProductSales",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ProductSaleId",
                table: "ProductRefunds",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductRefunds_ProductSaleId",
                table: "ProductRefunds",
                column: "ProductSaleId",
                unique: true,
                filter: "[ProductSaleId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductRefunds_ProductSales_ProductSaleId",
                table: "ProductRefunds",
                column: "ProductSaleId",
                principalTable: "ProductSales",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductRefunds_ProductSales_ProductSaleId",
                table: "ProductRefunds");

            migrationBuilder.DropIndex(
                name: "IX_ProductRefunds_ProductSaleId",
                table: "ProductRefunds");

            migrationBuilder.DropColumn(
                name: "IsRefund",
                table: "ProductSales");

            migrationBuilder.DropColumn(
                name: "ProductSaleId",
                table: "ProductRefunds");
        }
    }
}

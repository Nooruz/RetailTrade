using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetailTrade.EntityFramework.Migrations
{
    public partial class ProductSaleToPointSale : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SalePrice",
                table: "ProductSales",
                newName: "RetailPrice");

            migrationBuilder.RenameColumn(
                name: "ArrivalPrice",
                table: "ProductSales",
                newName: "PurchasePrice");

            migrationBuilder.AddColumn<int>(
                name: "PointSaleId",
                table: "ProductSales",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductSales_PointSaleId",
                table: "ProductSales",
                column: "PointSaleId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSales_PointSales_PointSaleId",
                table: "ProductSales",
                column: "PointSaleId",
                principalTable: "PointSales",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductSales_PointSales_PointSaleId",
                table: "ProductSales");

            migrationBuilder.DropIndex(
                name: "IX_ProductSales_PointSaleId",
                table: "ProductSales");

            migrationBuilder.DropColumn(
                name: "PointSaleId",
                table: "ProductSales");

            migrationBuilder.RenameColumn(
                name: "RetailPrice",
                table: "ProductSales",
                newName: "SalePrice");

            migrationBuilder.RenameColumn(
                name: "PurchasePrice",
                table: "ProductSales",
                newName: "ArrivalPrice");
        }
    }
}

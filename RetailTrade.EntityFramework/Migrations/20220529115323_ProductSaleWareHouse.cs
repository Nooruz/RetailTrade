using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetailTrade.EntityFramework.Migrations
{
    public partial class ProductSaleWareHouse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WareHouseId",
                table: "ProductSales",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductSales_WareHouseId",
                table: "ProductSales",
                column: "WareHouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSales_WareHouses_WareHouseId",
                table: "ProductSales",
                column: "WareHouseId",
                principalTable: "WareHouses",
                principalColumn: "Id");

            migrationBuilder.Sql(@"CREATE VIEW ProductWareHouseView AS SELECT dbo.Products.Id, 
                         dbo.Products.Name, 
                         dbo.TypeProducts.Name AS Supplier, 
                         dbo.Units.ShortName AS Unit, 
                         dbo.Products.TNVED, 
                         dbo.Products.Barcode, 
                         dbo.Products.ArrivalPrice, 
                         dbo.Products.SalePrice, 
                         dbo.ProductsWareHouses.Quantity
                         FROM dbo.Products INNER JOIN
                         dbo.ProductsWareHouses ON dbo.Products.Id = dbo.ProductsWareHouses.ProductId INNER JOIN
                         dbo.WareHouses ON dbo.ProductsWareHouses.WareHouseId = dbo.WareHouses.Id INNER JOIN
                         dbo.TypeProducts ON dbo.Products.TypeProductId = dbo.TypeProducts.Id INNER JOIN
                         dbo.Units ON dbo.Products.UnitId = dbo.Units.Id
                         WHERE (dbo.Products.DeleteMark = 0) AND (dbo.ProductsWareHouses.Quantity > 0)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductSales_WareHouses_WareHouseId",
                table: "ProductSales");

            migrationBuilder.DropIndex(
                name: "IX_ProductSales_WareHouseId",
                table: "ProductSales");

            migrationBuilder.DropColumn(
                name: "WareHouseId",
                table: "ProductSales");
        }
    }
}

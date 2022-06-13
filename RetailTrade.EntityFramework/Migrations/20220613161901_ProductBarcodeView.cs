using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetailTrade.EntityFramework.Migrations
{
    public partial class ProductBarcodeView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE VIEW ProductBarcodeView AS SELECT 
                                    dbo.ProductBarcode.Id,
                                    dbo.ProductBarcode.ProductId,
                                    dbo.Products.Name, 
                                    dbo.ProductBarcode.Barcode
                                    FROM dbo.Products INNER JOIN
                                    dbo.ProductBarcode ON dbo.Products.Id = dbo.ProductBarcode.ProductId
                                    WHERE (dbo.Products.DeleteMark = 0)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetailTrade.EntityFramework.Migrations
{
    public partial class UpdateProductView1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP VIEW [dbo].[ProductView]");

            migrationBuilder.Sql(@"CREATE VIEW ProductView AS SELECT dbo.Products.Id, 
                                          dbo.Products.Name, 
                                          dbo.Units.ShortName AS Unit, 
                                          dbo.Products.Barcode, 
                                          dbo.Products.PurchasePrice, 
                                          dbo.Products.RetailPrice, 
                                          dbo.Products.TNVED, 
                                          dbo.Products.DeleteMark, 
                                          dbo.TypeProducts.Name AS TypeProduct
                                   FROM dbo.Products INNER JOIN
                                        dbo.Units ON dbo.Products.UnitId = dbo.Units.Id INNER JOIN
                                        dbo.TypeProducts ON dbo.Products.TypeProductId = dbo.TypeProducts.Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

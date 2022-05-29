using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetailTrade.EntityFramework.Migrations
{
    public partial class CreateProductView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE VIEW ProductView AS SELECT
                                        dbo.Products.Id, 
                                        dbo.Products.Name, 
                                        dbo.Units.ShortName AS Unit, 
                                        dbo.Products.Barcode, 
                                        dbo.Products.ArrivalPrice, 
                                        dbo.Products.SalePrice, 
                                        dbo.Products.TNVED, 
                                        dbo.Products.DeleteMark
                                    FROM dbo.Products INNER JOIN
                                        dbo.Units ON dbo.Products.UnitId = dbo.Units.Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

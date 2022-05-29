using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetailTrade.EntityFramework.Migrations
{
    public partial class CreateTriggerForProductSaleArrivalProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE TRIGGER OnProductSaleInsert 
                                   ON  [dbo].[ProductSales] 
                                   AFTER INSERT
                                    AS 
                                    BEGIN
	                                    UPDATE [dbo].[ProductsWareHouses]
	                                    SET Quantity = Quantity - (SELECT Quantity FROM inserted)
	                                    WHERE ProductId = (SELECt ProductId FROM inserted) AND WareHouseId = (SELECT WareHouseId FROM inserted)
                                    END
                                    GO");

            migrationBuilder.Sql(@"CREATE TRIGGER OnArrivalProductInsert 
                                   ON  [dbo].[ArrivalProducts] 
                                   AFTER INSERT
                                    AS 
                                    BEGIN
	                                    UPDATE [dbo].[ProductsWareHouses]
	                                    SET Quantity = Quantity + (SELECT Quantity FROM inserted)
	                                    WHERE ProductId = (SELECt ProductId FROM inserted) AND WareHouseId = (SELECT WareHouseId FROM inserted)
                                    END
                                    GO");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

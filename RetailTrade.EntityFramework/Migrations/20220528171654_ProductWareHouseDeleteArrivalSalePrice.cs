using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetailTrade.EntityFramework.Migrations
{
    public partial class ProductWareHouseDeleteArrivalSalePrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArrivalPrice",
                table: "ProductsWareHouses");

            migrationBuilder.DropColumn(
                name: "SalePrice",
                table: "ProductsWareHouses");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ArrivalPrice",
                table: "ProductsWareHouses",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SalePrice",
                table: "ProductsWareHouses",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}

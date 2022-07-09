using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetailTrade.EntityFramework.Migrations
{
    public partial class ProductFunction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "WareHouses",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "WareHouses",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Основной склад");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "WareHouses",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Розничный магазин");

            migrationBuilder.InsertData(
                table: "WareHouses",
                columns: new[] { "Id", "Address", "DeleteMark", "Name", "TypeWareHouseId" },
                values: new object[] { 2, null, false, "Основной склад", 0 });
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetailTrade.EntityFramework.Migrations
{
    public partial class ArrivalWareHouse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WareHouseId",
                table: "Arrivals",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Arrivals_WareHouseId",
                table: "Arrivals",
                column: "WareHouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Arrivals_WareHouses_WareHouseId",
                table: "Arrivals",
                column: "WareHouseId",
                principalTable: "WareHouses",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Arrivals_WareHouses_WareHouseId",
                table: "Arrivals");

            migrationBuilder.DropIndex(
                name: "IX_Arrivals_WareHouseId",
                table: "Arrivals");

            migrationBuilder.DropColumn(
                name: "WareHouseId",
                table: "Arrivals");
        }
    }
}

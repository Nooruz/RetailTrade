using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetailTrade.EntityFramework.Migrations
{
    public partial class WareHouseArrivalProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WareHouseId",
                table: "ArrivalProducts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArrivalProducts_WareHouseId",
                table: "ArrivalProducts",
                column: "WareHouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArrivalProducts_WareHouses_WareHouseId",
                table: "ArrivalProducts",
                column: "WareHouseId",
                principalTable: "WareHouses",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArrivalProducts_WareHouses_WareHouseId",
                table: "ArrivalProducts");

            migrationBuilder.DropIndex(
                name: "IX_ArrivalProducts_WareHouseId",
                table: "ArrivalProducts");

            migrationBuilder.DropColumn(
                name: "WareHouseId",
                table: "ArrivalProducts");
        }
    }
}

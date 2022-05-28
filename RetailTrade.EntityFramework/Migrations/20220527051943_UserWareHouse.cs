using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetailTrade.EntityFramework.Migrations
{
    public partial class UserWareHouse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "WareHouses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WareHouseId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_WareHouseId",
                table: "Users",
                column: "WareHouseId",
                unique: true,
                filter: "[WareHouseId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_WareHouses_WareHouseId",
                table: "Users",
                column: "WareHouseId",
                principalTable: "WareHouses",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_WareHouses_WareHouseId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_WareHouseId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "WareHouses");

            migrationBuilder.DropColumn(
                name: "WareHouseId",
                table: "Users");
        }
    }
}

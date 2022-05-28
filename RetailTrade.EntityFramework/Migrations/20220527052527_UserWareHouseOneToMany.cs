using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetailTrade.EntityFramework.Migrations
{
    public partial class UserWareHouseOneToMany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_WareHouseId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "WareHouses");

            migrationBuilder.CreateIndex(
                name: "IX_Users_WareHouseId",
                table: "Users",
                column: "WareHouseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_WareHouseId",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "WareHouses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Users_WareHouseId",
                table: "Users",
                column: "WareHouseId",
                unique: true,
                filter: "[WareHouseId] IS NOT NULL");
        }
    }
}

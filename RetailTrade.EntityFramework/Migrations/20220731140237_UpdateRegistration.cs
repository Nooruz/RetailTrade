using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetailTrade.EntityFramework.Migrations
{
    public partial class UpdateRegistration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RegistrationProduct_WareHouses_WareHouseId",
                table: "RegistrationProduct");

            migrationBuilder.DropIndex(
                name: "IX_RegistrationProduct_WareHouseId",
                table: "RegistrationProduct");

            migrationBuilder.DropColumn(
                name: "WareHouseId",
                table: "RegistrationProduct");

            migrationBuilder.AddColumn<int>(
                name: "WareHouseId",
                table: "Registration",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Registration_WareHouseId",
                table: "Registration",
                column: "WareHouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Registration_WareHouses_WareHouseId",
                table: "Registration",
                column: "WareHouseId",
                principalTable: "WareHouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Registration_WareHouses_WareHouseId",
                table: "Registration");

            migrationBuilder.DropIndex(
                name: "IX_Registration_WareHouseId",
                table: "Registration");

            migrationBuilder.DropColumn(
                name: "WareHouseId",
                table: "Registration");

            migrationBuilder.AddColumn<int>(
                name: "WareHouseId",
                table: "RegistrationProduct",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationProduct_WareHouseId",
                table: "RegistrationProduct",
                column: "WareHouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_RegistrationProduct_WareHouses_WareHouseId",
                table: "RegistrationProduct",
                column: "WareHouseId",
                principalTable: "WareHouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetailTrade.EntityFramework.Migrations
{
    public partial class UpdateArrival : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArrivalProducts_WareHouses_WareHouseId",
                table: "ArrivalProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductSales_Receipts_ReceiptId",
                table: "ProductSales");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductSales_WareHouses_WareHouseId",
                table: "ProductSales");

            migrationBuilder.DropIndex(
                name: "IX_ProductSales_WareHouseId",
                table: "ProductSales");

            migrationBuilder.DropIndex(
                name: "IX_ArrivalProducts_WareHouseId",
                table: "ArrivalProducts");

            migrationBuilder.DropColumn(
                name: "WareHouseId",
                table: "ProductSales");

            migrationBuilder.DropColumn(
                name: "WareHouseId",
                table: "ArrivalProducts");

            migrationBuilder.RenameColumn(
                name: "ArrivalPrice",
                table: "ArrivalProducts",
                newName: "PurchasePrice");

            migrationBuilder.AddColumn<int>(
                name: "WareHouseId",
                table: "Receipts",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ReceiptId",
                table: "ProductSales",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Receipts_WareHouseId",
                table: "Receipts",
                column: "WareHouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSales_Receipts_ReceiptId",
                table: "ProductSales",
                column: "ReceiptId",
                principalTable: "Receipts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Receipts_WareHouses_WareHouseId",
                table: "Receipts",
                column: "WareHouseId",
                principalTable: "WareHouses",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductSales_Receipts_ReceiptId",
                table: "ProductSales");

            migrationBuilder.DropForeignKey(
                name: "FK_Receipts_WareHouses_WareHouseId",
                table: "Receipts");

            migrationBuilder.DropIndex(
                name: "IX_Receipts_WareHouseId",
                table: "Receipts");

            migrationBuilder.DropColumn(
                name: "WareHouseId",
                table: "Receipts");

            migrationBuilder.RenameColumn(
                name: "PurchasePrice",
                table: "ArrivalProducts",
                newName: "ArrivalPrice");

            migrationBuilder.AlterColumn<int>(
                name: "ReceiptId",
                table: "ProductSales",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WareHouseId",
                table: "ProductSales",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WareHouseId",
                table: "ArrivalProducts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductSales_WareHouseId",
                table: "ProductSales",
                column: "WareHouseId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSales_Receipts_ReceiptId",
                table: "ProductSales",
                column: "ReceiptId",
                principalTable: "Receipts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSales_WareHouses_WareHouseId",
                table: "ProductSales",
                column: "WareHouseId",
                principalTable: "WareHouses",
                principalColumn: "Id");
        }
    }
}

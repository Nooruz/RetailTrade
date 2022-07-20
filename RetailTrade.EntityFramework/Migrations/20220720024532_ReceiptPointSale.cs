using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetailTrade.EntityFramework.Migrations
{
    public partial class ReceiptPointSale : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PointSaleId",
                table: "Receipts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Receipts_PointSaleId",
                table: "Receipts",
                column: "PointSaleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Receipts_PointSales_PointSaleId",
                table: "Receipts",
                column: "PointSaleId",
                principalTable: "PointSales",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Receipts_PointSales_PointSaleId",
                table: "Receipts");

            migrationBuilder.DropIndex(
                name: "IX_Receipts_PointSaleId",
                table: "Receipts");

            migrationBuilder.DropColumn(
                name: "PointSaleId",
                table: "Receipts");
        }
    }
}

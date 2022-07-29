using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetailTrade.EntityFramework.Migrations
{
    public partial class DeleteWareHouse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SalePrice",
                table: "ProductRefunds",
                newName: "RetailPrice");

            migrationBuilder.RenameColumn(
                name: "ArrivalPrice",
                table: "ProductRefunds",
                newName: "PurchasePrice");

            migrationBuilder.AddColumn<int>(
                name: "ReceiptId",
                table: "Refunds",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Refunds_ReceiptId",
                table: "Refunds",
                column: "ReceiptId",
                unique: true,
                filter: "[ReceiptId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Refunds_Receipts_ReceiptId",
                table: "Refunds",
                column: "ReceiptId",
                principalTable: "Receipts",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Refunds_Receipts_ReceiptId",
                table: "Refunds");

            migrationBuilder.DropIndex(
                name: "IX_Refunds_ReceiptId",
                table: "Refunds");

            migrationBuilder.DropColumn(
                name: "ReceiptId",
                table: "Refunds");

            migrationBuilder.RenameColumn(
                name: "RetailPrice",
                table: "ProductRefunds",
                newName: "SalePrice");

            migrationBuilder.RenameColumn(
                name: "PurchasePrice",
                table: "ProductRefunds",
                newName: "ArrivalPrice");
        }
    }
}

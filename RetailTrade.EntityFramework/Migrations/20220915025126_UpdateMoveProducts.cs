using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetailTrade.EntityFramework.Migrations
{
    public partial class UpdateMoveProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MoveProduct_Documents_DocumentId",
                table: "MoveProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_MoveProduct_Products_ProductId",
                table: "MoveProduct");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MoveProduct",
                table: "MoveProduct");

            migrationBuilder.RenameTable(
                name: "MoveProduct",
                newName: "MoveProducts");

            migrationBuilder.RenameIndex(
                name: "IX_MoveProduct_ProductId",
                table: "MoveProducts",
                newName: "IX_MoveProducts_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_MoveProduct_DocumentId",
                table: "MoveProducts",
                newName: "IX_MoveProducts_DocumentId");

            migrationBuilder.AddColumn<double>(
                name: "StockTo",
                table: "MoveProducts",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MoveProducts",
                table: "MoveProducts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MoveProducts_Documents_DocumentId",
                table: "MoveProducts",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MoveProducts_Products_ProductId",
                table: "MoveProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MoveProducts_Documents_DocumentId",
                table: "MoveProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_MoveProducts_Products_ProductId",
                table: "MoveProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MoveProducts",
                table: "MoveProducts");

            migrationBuilder.DropColumn(
                name: "StockTo",
                table: "MoveProducts");

            migrationBuilder.RenameTable(
                name: "MoveProducts",
                newName: "MoveProduct");

            migrationBuilder.RenameIndex(
                name: "IX_MoveProducts_ProductId",
                table: "MoveProduct",
                newName: "IX_MoveProduct_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_MoveProducts_DocumentId",
                table: "MoveProduct",
                newName: "IX_MoveProduct_DocumentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MoveProduct",
                table: "MoveProduct",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MoveProduct_Documents_DocumentId",
                table: "MoveProduct",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MoveProduct_Products_ProductId",
                table: "MoveProduct",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

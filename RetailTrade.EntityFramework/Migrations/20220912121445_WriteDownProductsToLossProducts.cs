using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetailTrade.EntityFramework.Migrations
{
    public partial class WriteDownProductsToLossProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WriteDownProducts_Documents_DocumentId",
                table: "WriteDownProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_WriteDownProducts_Products_ProductId",
                table: "WriteDownProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_WriteDownProducts_WriteDowns_WriteDownId",
                table: "WriteDownProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WriteDownProducts",
                table: "WriteDownProducts");

            migrationBuilder.RenameTable(
                name: "WriteDownProducts",
                newName: "LossProducts");

            migrationBuilder.RenameIndex(
                name: "IX_WriteDownProducts_WriteDownId",
                table: "LossProducts",
                newName: "IX_LossProducts_WriteDownId");

            migrationBuilder.RenameIndex(
                name: "IX_WriteDownProducts_ProductId",
                table: "LossProducts",
                newName: "IX_LossProducts_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_WriteDownProducts_DocumentId",
                table: "LossProducts",
                newName: "IX_LossProducts_DocumentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LossProducts",
                table: "LossProducts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LossProducts_Documents_DocumentId",
                table: "LossProducts",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LossProducts_Products_ProductId",
                table: "LossProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LossProducts_WriteDowns_WriteDownId",
                table: "LossProducts",
                column: "WriteDownId",
                principalTable: "WriteDowns",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LossProducts_Documents_DocumentId",
                table: "LossProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_LossProducts_Products_ProductId",
                table: "LossProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_LossProducts_WriteDowns_WriteDownId",
                table: "LossProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LossProducts",
                table: "LossProducts");

            migrationBuilder.RenameTable(
                name: "LossProducts",
                newName: "WriteDownProducts");

            migrationBuilder.RenameIndex(
                name: "IX_LossProducts_WriteDownId",
                table: "WriteDownProducts",
                newName: "IX_WriteDownProducts_WriteDownId");

            migrationBuilder.RenameIndex(
                name: "IX_LossProducts_ProductId",
                table: "WriteDownProducts",
                newName: "IX_WriteDownProducts_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_LossProducts_DocumentId",
                table: "WriteDownProducts",
                newName: "IX_WriteDownProducts_DocumentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WriteDownProducts",
                table: "WriteDownProducts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WriteDownProducts_Documents_DocumentId",
                table: "WriteDownProducts",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WriteDownProducts_Products_ProductId",
                table: "WriteDownProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WriteDownProducts_WriteDowns_WriteDownId",
                table: "WriteDownProducts",
                column: "WriteDownId",
                principalTable: "WriteDowns",
                principalColumn: "Id");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace RetailTrade.EntityFramework.Migrations
{
    public partial class UpdateArrivalAndWriteDownProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Barcode",
                table: "WriteDownProducts");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "WriteDownProducts");

            migrationBuilder.DropColumn(
                name: "QuantityInStock",
                table: "WriteDownProducts");

            migrationBuilder.DropColumn(
                name: "Barcode",
                table: "ArrivalProducts");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "WriteDownProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_WriteDownProducts_ProductId",
                table: "WriteDownProducts",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_WriteDownProducts_Products_ProductId",
                table: "WriteDownProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WriteDownProducts_Products_ProductId",
                table: "WriteDownProducts");

            migrationBuilder.DropIndex(
                name: "IX_WriteDownProducts_ProductId",
                table: "WriteDownProducts");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "WriteDownProducts");

            migrationBuilder.AddColumn<string>(
                name: "Barcode",
                table: "WriteDownProducts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "WriteDownProducts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "QuantityInStock",
                table: "WriteDownProducts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Barcode",
                table: "ArrivalProducts",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetailTrade.EntityFramework.Migrations
{
    public partial class DeleteProductPrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductPrices_ProductPriceId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "ProductPrices");

            migrationBuilder.DropIndex(
                name: "IX_Products_ProductPriceId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductPriceId",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "SalePrice",
                table: "Products",
                newName: "RetailPrice");

            migrationBuilder.RenameColumn(
                name: "ArrivalPrice",
                table: "Products",
                newName: "PurchasePrice");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RetailPrice",
                table: "Products",
                newName: "SalePrice");

            migrationBuilder.RenameColumn(
                name: "PurchasePrice",
                table: "Products",
                newName: "ArrivalPrice");

            migrationBuilder.AddColumn<int>(
                name: "ProductPriceId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProductPrices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CostPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MinimumPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RetailPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WholesalePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPrices", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductPriceId",
                table: "Products",
                column: "ProductPriceId",
                unique: true,
                filter: "[ProductPriceId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductPrices_ProductPriceId",
                table: "Products",
                column: "ProductPriceId",
                principalTable: "ProductPrices",
                principalColumn: "Id");
        }
    }
}

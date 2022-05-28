using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetailTrade.EntityFramework.Migrations
{
    public partial class ProductsWareHouses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductWareHouse");

            migrationBuilder.CreateTable(
                name: "ProductsWareHouses",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    WareHouseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsWareHouses", x => new { x.ProductId, x.WareHouseId });
                    table.ForeignKey(
                        name: "FK_ProductsWareHouses_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductsWareHouses_WareHouses_WareHouseId",
                        column: x => x.WareHouseId,
                        principalTable: "WareHouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductsWareHouses_WareHouseId",
                table: "ProductsWareHouses",
                column: "WareHouseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductsWareHouses");

            migrationBuilder.CreateTable(
                name: "ProductWareHouse",
                columns: table => new
                {
                    ProductsId = table.Column<int>(type: "int", nullable: false),
                    WareHousesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductWareHouse", x => new { x.ProductsId, x.WareHousesId });
                    table.ForeignKey(
                        name: "FK_ProductWareHouse_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductWareHouse_WareHouses_WareHousesId",
                        column: x => x.WareHousesId,
                        principalTable: "WareHouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductWareHouse_WareHousesId",
                table: "ProductWareHouse",
                column: "WareHousesId");
        }
    }
}

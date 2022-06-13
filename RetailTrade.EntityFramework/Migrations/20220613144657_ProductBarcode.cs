using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetailTrade.EntityFramework.Migrations
{
    public partial class ProductBarcode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TypePrices",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "TypePrices",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.CreateTable(
                name: "ProductBarcode",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Barcode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductBarcode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductBarcode_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "TypePrices",
                keyColumn: "Id",
                keyValue: 4,
                column: "Name",
                value: "Минимальная");

            migrationBuilder.CreateIndex(
                name: "IX_ProductBarcode_ProductId",
                table: "ProductBarcode",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductBarcode");

            migrationBuilder.UpdateData(
                table: "TypePrices",
                keyColumn: "Id",
                keyValue: 4,
                column: "Name",
                value: "Мелкооптовая");

            migrationBuilder.InsertData(
                table: "TypePrices",
                columns: new[] { "Id", "Name" },
                values: new object[] { 5, "Максимальная" });

            migrationBuilder.InsertData(
                table: "TypePrices",
                columns: new[] { "Id", "Name" },
                values: new object[] { 6, "Минимальная" });
        }
    }
}

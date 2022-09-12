using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetailTrade.EntityFramework.Migrations
{
    public partial class MoveProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Stock",
                table: "LossProducts",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Stock",
                table: "EnterProduct",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "ToWareHouseId",
                table: "Documents",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MoveProduct",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    DocumentId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<double>(type: "float", nullable: false),
                    Stock = table.Column<double>(type: "float", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoveProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MoveProduct_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MoveProduct_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MoveProduct_DocumentId",
                table: "MoveProduct",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_MoveProduct_ProductId",
                table: "MoveProduct",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MoveProduct");

            migrationBuilder.DropColumn(
                name: "Stock",
                table: "LossProducts");

            migrationBuilder.DropColumn(
                name: "Stock",
                table: "EnterProduct");

            migrationBuilder.DropColumn(
                name: "ToWareHouseId",
                table: "Documents");
        }
    }
}

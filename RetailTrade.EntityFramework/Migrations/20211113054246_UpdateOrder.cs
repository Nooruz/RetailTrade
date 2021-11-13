using Microsoft.EntityFrameworkCore.Migrations;

namespace RetailTrade.EntityFramework.Migrations
{
    public partial class UpdateOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderProductOrderToSupplier");

            migrationBuilder.DropColumn(
                name: "OrderProductId",
                table: "OrdersToSuppliers");

            migrationBuilder.AddColumn<int>(
                name: "OrderToSupplierId",
                table: "OrderProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OrderProducts_OrderToSupplierId",
                table: "OrderProducts",
                column: "OrderToSupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderProducts_OrdersToSuppliers_OrderToSupplierId",
                table: "OrderProducts",
                column: "OrderToSupplierId",
                principalTable: "OrdersToSuppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderProducts_OrdersToSuppliers_OrderToSupplierId",
                table: "OrderProducts");

            migrationBuilder.DropIndex(
                name: "IX_OrderProducts_OrderToSupplierId",
                table: "OrderProducts");

            migrationBuilder.DropColumn(
                name: "OrderToSupplierId",
                table: "OrderProducts");

            migrationBuilder.AddColumn<int>(
                name: "OrderProductId",
                table: "OrdersToSuppliers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "OrderProductOrderToSupplier",
                columns: table => new
                {
                    OrderProductsId = table.Column<int>(type: "int", nullable: false),
                    OrderToSuppliersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderProductOrderToSupplier", x => new { x.OrderProductsId, x.OrderToSuppliersId });
                    table.ForeignKey(
                        name: "FK_OrderProductOrderToSupplier_OrderProducts_OrderProductsId",
                        column: x => x.OrderProductsId,
                        principalTable: "OrderProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderProductOrderToSupplier_OrdersToSuppliers_OrderToSuppliersId",
                        column: x => x.OrderToSuppliersId,
                        principalTable: "OrdersToSuppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderProductOrderToSupplier_OrderToSuppliersId",
                table: "OrderProductOrderToSupplier",
                column: "OrderToSuppliersId");
        }
    }
}

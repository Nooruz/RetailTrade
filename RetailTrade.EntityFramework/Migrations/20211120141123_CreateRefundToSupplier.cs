using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RetailTrade.EntityFramework.Migrations
{
    public partial class CreateRefundToSupplier : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductRefunds");

            migrationBuilder.CreateTable(
                name: "RefundsToSuppliers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RefundToSupplierDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefundsToSuppliers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefundsToSuppliers_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefundToSupplierProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RefundToSupplierId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefundToSupplierProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefundToSupplierProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RefundToSupplierProducts_RefundsToSuppliers_RefundToSupplierId",
                        column: x => x.RefundToSupplierId,
                        principalTable: "RefundsToSuppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RefundsToSuppliers_SupplierId",
                table: "RefundsToSuppliers",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_RefundToSupplierProducts_ProductId",
                table: "RefundToSupplierProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_RefundToSupplierProducts_RefundToSupplierId",
                table: "RefundToSupplierProducts",
                column: "RefundToSupplierId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefundToSupplierProducts");

            migrationBuilder.DropTable(
                name: "RefundsToSuppliers");

            migrationBuilder.CreateTable(
                name: "ProductRefunds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RefundDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductRefunds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductRefunds_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductRefunds_ProductId",
                table: "ProductRefunds",
                column: "ProductId");
        }
    }
}

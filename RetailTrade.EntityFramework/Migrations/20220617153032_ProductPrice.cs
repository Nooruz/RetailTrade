﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetailTrade.EntityFramework.Migrations
{
    public partial class ProductPrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PriceProducts");

            migrationBuilder.DropTable(
                name: "TypePrices");

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
                    RetailPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CostPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WholesalePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MinimumPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "TypePrices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypePrices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PriceProducts",
                columns: table => new
                {
                    TypePriceId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceProducts", x => x.TypePriceId);
                    table.ForeignKey(
                        name: "FK_PriceProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PriceProducts_TypePrices_TypePriceId",
                        column: x => x.TypePriceId,
                        principalTable: "TypePrices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "TypePrices",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Розничная" },
                    { 2, "Себестоимость" },
                    { 3, "Оптовая" },
                    { 4, "Минимальная" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PriceProducts_ProductId",
                table: "PriceProducts",
                column: "ProductId");
        }
    }
}
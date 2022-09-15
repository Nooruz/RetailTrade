using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetailTrade.EntityFramework.Migrations
{
    public partial class DeleteAllDocumentProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EnterProducts");

            migrationBuilder.DropTable(
                name: "LossProducts");

            migrationBuilder.DropTable(
                name: "MoveProducts");

            migrationBuilder.DropTable(
                name: "ProductWareHouse");

            migrationBuilder.DropTable(
                name: "Registrations");

            migrationBuilder.AddColumn<int>(
                name: "WareHouseId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DocumentProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    DocumentId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<double>(type: "float", nullable: false),
                    Stock = table.Column<double>(type: "float", nullable: false),
                    StockTo = table.Column<double>(type: "float", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentProducts_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocumentProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_WareHouseId",
                table: "Products",
                column: "WareHouseId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentProducts_DocumentId",
                table: "DocumentProducts",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentProducts_ProductId",
                table: "DocumentProducts",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_WareHouses_WareHouseId",
                table: "Products",
                column: "WareHouseId",
                principalTable: "WareHouses",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_WareHouses_WareHouseId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "DocumentProducts");

            migrationBuilder.DropIndex(
                name: "IX_Products_WareHouseId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "WareHouseId",
                table: "Products");

            migrationBuilder.CreateTable(
                name: "LossProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<double>(type: "float", nullable: false),
                    Stock = table.Column<double>(type: "float", nullable: false),
                    WriteDownId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LossProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LossProducts_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LossProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LossProducts_WriteDowns_WriteDownId",
                        column: x => x.WriteDownId,
                        principalTable: "WriteDowns",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MoveProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<double>(type: "float", nullable: false),
                    Stock = table.Column<double>(type: "float", nullable: false),
                    StockTo = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoveProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MoveProducts_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MoveProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateTable(
                name: "Registrations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WareHouseId = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Sum = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registrations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Registrations_WareHouses_WareHouseId",
                        column: x => x.WareHouseId,
                        principalTable: "WareHouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EnterProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<double>(type: "float", nullable: false),
                    RegistrationId = table.Column<int>(type: "int", nullable: true),
                    Stock = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnterProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnterProducts_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EnterProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EnterProducts_Registrations_RegistrationId",
                        column: x => x.RegistrationId,
                        principalTable: "Registrations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_EnterProducts_DocumentId",
                table: "EnterProducts",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_EnterProducts_ProductId",
                table: "EnterProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_EnterProducts_RegistrationId",
                table: "EnterProducts",
                column: "RegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_LossProducts_DocumentId",
                table: "LossProducts",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_LossProducts_ProductId",
                table: "LossProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_LossProducts_WriteDownId",
                table: "LossProducts",
                column: "WriteDownId");

            migrationBuilder.CreateIndex(
                name: "IX_MoveProducts_DocumentId",
                table: "MoveProducts",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_MoveProducts_ProductId",
                table: "MoveProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductWareHouse_WareHousesId",
                table: "ProductWareHouse",
                column: "WareHousesId");

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_WareHouseId",
                table: "Registrations",
                column: "WareHouseId");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetailTrade.EntityFramework.Migrations
{
    public partial class RegistrationProductToEnter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_WareHouses_WareHouseId",
                table: "Documents");

            migrationBuilder.DropTable(
                name: "RegistrationProduct");

            migrationBuilder.AlterColumn<int>(
                name: "WareHouseId",
                table: "Documents",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "EnterProduct",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    DocumentId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<double>(type: "float", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RegistrationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnterProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnterProduct_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EnterProduct_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EnterProduct_Registrations_RegistrationId",
                        column: x => x.RegistrationId,
                        principalTable: "Registrations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_EnterProduct_DocumentId",
                table: "EnterProduct",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_EnterProduct_ProductId",
                table: "EnterProduct",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_EnterProduct_RegistrationId",
                table: "EnterProduct",
                column: "RegistrationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_WareHouses_WareHouseId",
                table: "Documents",
                column: "WareHouseId",
                principalTable: "WareHouses",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_WareHouses_WareHouseId",
                table: "Documents");

            migrationBuilder.DropTable(
                name: "EnterProduct");

            migrationBuilder.AlterColumn<int>(
                name: "WareHouseId",
                table: "Documents",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "RegistrationProduct",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    RegistrationId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrationProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistrationProduct_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegistrationProduct_Registrations_RegistrationId",
                        column: x => x.RegistrationId,
                        principalTable: "Registrations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationProduct_ProductId",
                table: "RegistrationProduct",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationProduct_RegistrationId",
                table: "RegistrationProduct",
                column: "RegistrationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_WareHouses_WareHouseId",
                table: "Documents",
                column: "WareHouseId",
                principalTable: "WareHouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

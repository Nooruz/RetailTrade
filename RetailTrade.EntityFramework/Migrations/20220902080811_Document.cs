using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetailTrade.EntityFramework.Migrations
{
    public partial class Document : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Registration_WareHouses_WareHouseId",
                table: "Registration");

            migrationBuilder.DropForeignKey(
                name: "FK_RegistrationProduct_Registration_RegistrationId",
                table: "RegistrationProduct");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Registration",
                table: "Registration");

            migrationBuilder.RenameTable(
                name: "Registration",
                newName: "Registrations");

            migrationBuilder.RenameIndex(
                name: "IX_Registration_WareHouseId",
                table: "Registrations",
                newName: "IX_Registrations_WareHouseId");

            migrationBuilder.AddColumn<int>(
                name: "DocumentId",
                table: "ProductSales",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Registrations",
                table: "Registrations",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "DocumentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentTypeId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WareHouseId = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documents_DocumentTypes_DocumentTypeId",
                        column: x => x.DocumentTypeId,
                        principalTable: "DocumentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Documents_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Documents_WareHouses_WareHouseId",
                        column: x => x.WareHouseId,
                        principalTable: "WareHouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "DocumentTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Приемка" },
                    { 2, "Возврат поставщику" },
                    { 3, "Списание" },
                    { 4, "Оприходование" },
                    { 5, "Перемещение" },
                    { 6, "Инвентаризация" },
                    { 7, "Приходный ордер" },
                    { 8, "Расходный ордер" },
                    { 9, "Корректировка остатков в кассе" },
                    { 10, "Корректировка остатков на счете" },
                    { 11, "Продажа" },
                    { 12, "Возврат" },
                    { 13, "Смена" },
                    { 14, "Внесение" },
                    { 15, "Выплата" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductSales_DocumentId",
                table: "ProductSales",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_DocumentTypeId",
                table: "Documents",
                column: "DocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_UserId",
                table: "Documents",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_WareHouseId",
                table: "Documents",
                column: "WareHouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSales_Documents_DocumentId",
                table: "ProductSales",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RegistrationProduct_Registrations_RegistrationId",
                table: "RegistrationProduct",
                column: "RegistrationId",
                principalTable: "Registrations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Registrations_WareHouses_WareHouseId",
                table: "Registrations",
                column: "WareHouseId",
                principalTable: "WareHouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductSales_Documents_DocumentId",
                table: "ProductSales");

            migrationBuilder.DropForeignKey(
                name: "FK_RegistrationProduct_Registrations_RegistrationId",
                table: "RegistrationProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_Registrations_WareHouses_WareHouseId",
                table: "Registrations");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "DocumentTypes");

            migrationBuilder.DropIndex(
                name: "IX_ProductSales_DocumentId",
                table: "ProductSales");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Registrations",
                table: "Registrations");

            migrationBuilder.DropColumn(
                name: "DocumentId",
                table: "ProductSales");

            migrationBuilder.RenameTable(
                name: "Registrations",
                newName: "Registration");

            migrationBuilder.RenameIndex(
                name: "IX_Registrations_WareHouseId",
                table: "Registration",
                newName: "IX_Registration_WareHouseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Registration",
                table: "Registration",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Registration_WareHouses_WareHouseId",
                table: "Registration",
                column: "WareHouseId",
                principalTable: "WareHouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RegistrationProduct_Registration_RegistrationId",
                table: "RegistrationProduct",
                column: "RegistrationId",
                principalTable: "Registration",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

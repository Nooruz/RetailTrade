using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetailTrade.EntityFramework.Migrations
{
    public partial class Revaluation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Revaluations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RevaluationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Revaluations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RevaluationProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    RevaluationId = table.Column<int>(type: "int", nullable: false),
                    ArrivalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SalePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RevaluationProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RevaluationProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RevaluationProducts_Revaluations_RevaluationId",
                        column: x => x.RevaluationId,
                        principalTable: "Revaluations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RevaluationProducts_ProductId",
                table: "RevaluationProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_RevaluationProducts_RevaluationId",
                table: "RevaluationProducts",
                column: "RevaluationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RevaluationProducts");

            migrationBuilder.DropTable(
                name: "Revaluations");
        }
    }
}

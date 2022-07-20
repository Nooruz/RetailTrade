using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetailTrade.EntityFramework.Migrations
{
    public partial class ShiftPointSale : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PointSaleId",
                table: "Shifts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shifts_PointSaleId",
                table: "Shifts",
                column: "PointSaleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shifts_PointSales_PointSaleId",
                table: "Shifts",
                column: "PointSaleId",
                principalTable: "PointSales",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shifts_PointSales_PointSaleId",
                table: "Shifts");

            migrationBuilder.DropIndex(
                name: "IX_Shifts_PointSaleId",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "PointSaleId",
                table: "Shifts");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetailTrade.EntityFramework.Migrations
{
    public partial class PointOfSale : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PointSales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AllowDiscount = table.Column<bool>(type: "bit", nullable: false),
                    MaximumDiscount = table.Column<double>(type: "float", nullable: false),
                    WareHouseId = table.Column<int>(type: "int", nullable: false),
                    AccountingBalances = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PointSales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PointSales_WareHouses_WareHouseId",
                        column: x => x.WareHouseId,
                        principalTable: "WareHouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPointSales",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PointSaleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPointSales", x => new { x.PointSaleId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserPointSales_PointSales_PointSaleId",
                        column: x => x.PointSaleId,
                        principalTable: "PointSales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPointSales_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PointSales_WareHouseId",
                table: "PointSales",
                column: "WareHouseId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPointSales_UserId",
                table: "UserPointSales",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserPointSales");

            migrationBuilder.DropTable(
                name: "PointSales");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RetailTrade.EntityFramework.Migrations
{
    public partial class CreateArrival : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArrivalDate",
                table: "ArrivalProducts");

            migrationBuilder.AddColumn<int>(
                name: "ArrivalId",
                table: "ArrivalProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Arrivals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArrivalDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Arrivals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Arrivals_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArrivalProducts_ArrivalId",
                table: "ArrivalProducts",
                column: "ArrivalId");

            migrationBuilder.CreateIndex(
                name: "IX_Arrivals_SupplierId",
                table: "Arrivals",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArrivalProducts_Arrivals_ArrivalId",
                table: "ArrivalProducts",
                column: "ArrivalId",
                principalTable: "Arrivals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArrivalProducts_Arrivals_ArrivalId",
                table: "ArrivalProducts");

            migrationBuilder.DropTable(
                name: "Arrivals");

            migrationBuilder.DropIndex(
                name: "IX_ArrivalProducts_ArrivalId",
                table: "ArrivalProducts");

            migrationBuilder.DropColumn(
                name: "ArrivalId",
                table: "ArrivalProducts");

            migrationBuilder.AddColumn<DateTime>(
                name: "ArrivalDate",
                table: "ArrivalProducts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}

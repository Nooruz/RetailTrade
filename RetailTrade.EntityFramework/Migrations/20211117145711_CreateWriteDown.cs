using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RetailTrade.EntityFramework.Migrations
{
    public partial class CreateWriteDown : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WriteDownDate",
                table: "WriteDownProducts");

            migrationBuilder.AddColumn<int>(
                name: "WriteDownId",
                table: "WriteDownProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "WriteDowns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WriteDownDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WriteDowns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WriteDowns_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WriteDownProducts_WriteDownId",
                table: "WriteDownProducts",
                column: "WriteDownId");

            migrationBuilder.CreateIndex(
                name: "IX_WriteDowns_SupplierId",
                table: "WriteDowns",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_WriteDownProducts_WriteDowns_WriteDownId",
                table: "WriteDownProducts",
                column: "WriteDownId",
                principalTable: "WriteDowns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WriteDownProducts_WriteDowns_WriteDownId",
                table: "WriteDownProducts");

            migrationBuilder.DropTable(
                name: "WriteDowns");

            migrationBuilder.DropIndex(
                name: "IX_WriteDownProducts_WriteDownId",
                table: "WriteDownProducts");

            migrationBuilder.DropColumn(
                name: "WriteDownId",
                table: "WriteDownProducts");

            migrationBuilder.AddColumn<DateTime>(
                name: "WriteDownDate",
                table: "WriteDownProducts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}

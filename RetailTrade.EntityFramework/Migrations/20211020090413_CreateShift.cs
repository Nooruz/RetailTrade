using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RetailTrade.EntityFramework.Migrations
{
    public partial class CreateShift : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Receipts_Users_UserId",
                table: "Receipts");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Receipts",
                newName: "ShiftId");

            migrationBuilder.RenameIndex(
                name: "IX_Receipts_UserId",
                table: "Receipts",
                newName: "IX_Receipts_ShiftId");

            migrationBuilder.CreateTable(
                name: "Shift",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OpeningDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClosingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shift", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shift_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Shift_UserId",
                table: "Shift",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Receipts_Shift_ShiftId",
                table: "Receipts",
                column: "ShiftId",
                principalTable: "Shift",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Receipts_Shift_ShiftId",
                table: "Receipts");

            migrationBuilder.DropTable(
                name: "Shift");

            migrationBuilder.RenameColumn(
                name: "ShiftId",
                table: "Receipts",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Receipts_ShiftId",
                table: "Receipts",
                newName: "IX_Receipts_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Receipts_Users_UserId",
                table: "Receipts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

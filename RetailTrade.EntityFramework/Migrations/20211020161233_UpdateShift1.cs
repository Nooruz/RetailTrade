using Microsoft.EntityFrameworkCore.Migrations;

namespace RetailTrade.EntityFramework.Migrations
{
    public partial class UpdateShift1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Receipts_Shift_ShiftId",
                table: "Receipts");

            migrationBuilder.DropForeignKey(
                name: "FK_Shift_Users_UserId",
                table: "Shift");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Shift",
                table: "Shift");

            migrationBuilder.RenameTable(
                name: "Shift",
                newName: "Shifts");

            migrationBuilder.RenameIndex(
                name: "IX_Shift_UserId",
                table: "Shifts",
                newName: "IX_Shifts_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Shifts",
                table: "Shifts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Receipts_Shifts_ShiftId",
                table: "Receipts",
                column: "ShiftId",
                principalTable: "Shifts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Shifts_Users_UserId",
                table: "Shifts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Receipts_Shifts_ShiftId",
                table: "Receipts");

            migrationBuilder.DropForeignKey(
                name: "FK_Shifts_Users_UserId",
                table: "Shifts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Shifts",
                table: "Shifts");

            migrationBuilder.RenameTable(
                name: "Shifts",
                newName: "Shift");

            migrationBuilder.RenameIndex(
                name: "IX_Shifts_UserId",
                table: "Shift",
                newName: "IX_Shift_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Shift",
                table: "Shift",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Receipts_Shift_ShiftId",
                table: "Receipts",
                column: "ShiftId",
                principalTable: "Shift",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Shift_Users_UserId",
                table: "Shift",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

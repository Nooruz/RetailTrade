using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetailTrade.EntityFramework.Migrations
{
    public partial class WriteDownToLoss : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WriteDownProducts_WriteDowns_WriteDownId",
                table: "WriteDownProducts");

            migrationBuilder.AlterColumn<int>(
                name: "WriteDownId",
                table: "WriteDownProducts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "WriteDownProducts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "WriteDownProducts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DocumentId",
                table: "WriteDownProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "WriteDownProducts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_WriteDownProducts_DocumentId",
                table: "WriteDownProducts",
                column: "DocumentId");

            migrationBuilder.AddForeignKey(
                name: "FK_WriteDownProducts_Documents_DocumentId",
                table: "WriteDownProducts",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WriteDownProducts_WriteDowns_WriteDownId",
                table: "WriteDownProducts",
                column: "WriteDownId",
                principalTable: "WriteDowns",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WriteDownProducts_Documents_DocumentId",
                table: "WriteDownProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_WriteDownProducts_WriteDowns_WriteDownId",
                table: "WriteDownProducts");

            migrationBuilder.DropIndex(
                name: "IX_WriteDownProducts_DocumentId",
                table: "WriteDownProducts");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "WriteDownProducts");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "WriteDownProducts");

            migrationBuilder.DropColumn(
                name: "DocumentId",
                table: "WriteDownProducts");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "WriteDownProducts");

            migrationBuilder.AlterColumn<int>(
                name: "WriteDownId",
                table: "WriteDownProducts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WriteDownProducts_WriteDowns_WriteDownId",
                table: "WriteDownProducts",
                column: "WriteDownId",
                principalTable: "WriteDowns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

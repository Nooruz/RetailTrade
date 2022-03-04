using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetailTrade.EntityFramework.Migrations
{
    public partial class OldRevaluationProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "OldArrivalPrice",
                table: "RevaluationProducts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "OldSalePrice",
                table: "RevaluationProducts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "OrderStatuses",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Ожидается поступление");

            migrationBuilder.UpdateData(
                table: "OrderStatuses",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Закрыт");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OldArrivalPrice",
                table: "RevaluationProducts");

            migrationBuilder.DropColumn(
                name: "OldSalePrice",
                table: "RevaluationProducts");

            migrationBuilder.UpdateData(
                table: "OrderStatuses",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Ожидание");

            migrationBuilder.UpdateData(
                table: "OrderStatuses",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Выполнено");
        }
    }
}

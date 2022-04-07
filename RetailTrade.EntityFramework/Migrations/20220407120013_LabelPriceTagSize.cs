using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetailTrade.EntityFramework.Migrations
{
    public partial class LabelPriceTagSize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Height",
                table: "LabelPriceTags");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "LabelPriceTags");

            migrationBuilder.AddColumn<int>(
                name: "LabelPriceTagSizeId",
                table: "LabelPriceTags",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LabelPriceTagSizes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Width = table.Column<int>(type: "int", nullable: false),
                    Height = table.Column<int>(type: "int", nullable: false),
                    TypeLabelPriceTagId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabelPriceTagSizes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LabelPriceTagSizes_TypeLabelPriceTags_TypeLabelPriceTagId",
                        column: x => x.TypeLabelPriceTagId,
                        principalTable: "TypeLabelPriceTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "LabelPriceTagSizes",
                columns: new[] { "Id", "Height", "TypeLabelPriceTagId", "Width" },
                values: new object[] { 1, 250, 1, 460 });

            migrationBuilder.UpdateData(
                table: "LabelPriceTags",
                keyColumn: "Id",
                keyValue: 1,
                column: "LabelPriceTagSizeId",
                value: 1);

            migrationBuilder.CreateIndex(
                name: "IX_LabelPriceTags_LabelPriceTagSizeId",
                table: "LabelPriceTags",
                column: "LabelPriceTagSizeId");

            migrationBuilder.CreateIndex(
                name: "IX_LabelPriceTagSizes_TypeLabelPriceTagId",
                table: "LabelPriceTagSizes",
                column: "TypeLabelPriceTagId");

            migrationBuilder.AddForeignKey(
                name: "FK_LabelPriceTags_LabelPriceTagSizes_LabelPriceTagSizeId",
                table: "LabelPriceTags",
                column: "LabelPriceTagSizeId",
                principalTable: "LabelPriceTagSizes",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LabelPriceTags_LabelPriceTagSizes_LabelPriceTagSizeId",
                table: "LabelPriceTags");

            migrationBuilder.DropTable(
                name: "LabelPriceTagSizes");

            migrationBuilder.DropIndex(
                name: "IX_LabelPriceTags_LabelPriceTagSizeId",
                table: "LabelPriceTags");

            migrationBuilder.DropColumn(
                name: "LabelPriceTagSizeId",
                table: "LabelPriceTags");

            migrationBuilder.AddColumn<int>(
                name: "Height",
                table: "LabelPriceTags",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Width",
                table: "LabelPriceTags",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

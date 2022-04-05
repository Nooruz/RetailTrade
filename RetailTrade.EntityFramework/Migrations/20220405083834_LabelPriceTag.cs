using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetailTrade.EntityFramework.Migrations
{
    public partial class LabelPriceTag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TypeEquipments");

            migrationBuilder.CreateTable(
                name: "TypeLabelPriceTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeLabelPriceTags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LabelPriceTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeLabelPriceTagId = table.Column<int>(type: "int", nullable: false),
                    Width = table.Column<int>(type: "int", nullable: false),
                    Height = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabelPriceTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LabelPriceTags_TypeLabelPriceTags_TypeLabelPriceTagId",
                        column: x => x.TypeLabelPriceTagId,
                        principalTable: "TypeLabelPriceTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "TypeLabelPriceTags",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Этикетка для товаров" });

            migrationBuilder.InsertData(
                table: "TypeLabelPriceTags",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "Ценник для товаров" });

            migrationBuilder.InsertData(
                table: "LabelPriceTags",
                columns: new[] { "Id", "Height", "Name", "TypeLabelPriceTagId", "Width" },
                values: new object[] { 1, 0, "Этикетка для товара", 1, 0 });

            migrationBuilder.InsertData(
                table: "LabelPriceTags",
                columns: new[] { "Id", "Height", "Name", "TypeLabelPriceTagId", "Width" },
                values: new object[] { 2, 0, "Ценник для товара", 2, 0 });

            migrationBuilder.CreateIndex(
                name: "IX_LabelPriceTags_TypeLabelPriceTagId",
                table: "LabelPriceTags",
                column: "TypeLabelPriceTagId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LabelPriceTags");

            migrationBuilder.DropTable(
                name: "TypeLabelPriceTags");

            migrationBuilder.CreateTable(
                name: "TypeEquipments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeEquipments", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "TypeEquipments",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Сканеры штрихкода" },
                    { 2, "Контрольно-кассовая машина (ККМ)" },
                    { 3, "Принтеры чеков" },
                    { 4, "Принтеры этикеток" }
                });
        }
    }
}

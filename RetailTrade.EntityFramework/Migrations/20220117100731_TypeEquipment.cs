using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetailTrade.EntityFramework.Migrations
{
    public partial class TypeEquipment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                table: "GroupsEmployees",
                columns: new[] { "Id", "Name", "SubGroupId" },
                values: new object[,]
                {
                    { 2, "Руководство", 1 },
                    { 3, "Кассиры", 1 }
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TypeEquipments");

            migrationBuilder.DeleteData(
                table: "GroupsEmployees",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "GroupsEmployees",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetailTrade.EntityFramework.Migrations
{
    public partial class TypeProductUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TypeProducts_GroupTypeProducts_GroupTypeProductId",
                table: "TypeProducts");

            migrationBuilder.DropTable(
                name: "GroupTypeProducts");

            migrationBuilder.DropIndex(
                name: "IX_TypeProducts_GroupTypeProductId",
                table: "TypeProducts");

            migrationBuilder.DropColumn(
                name: "GroupTypeProductId",
                table: "TypeProducts");

            migrationBuilder.AddColumn<bool>(
                name: "IsGroup",
                table: "TypeProducts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "SubGroupId",
                table: "TypeProducts",
                type: "int",
                nullable: true);

            migrationBuilder.InsertData(
                table: "TypeProducts",
                columns: new[] { "Id", "IsGroup", "Name", "SubGroupId" },
                values: new object[] { 1, true, "Виды товаров", null });

            migrationBuilder.CreateIndex(
                name: "IX_TypeProducts_SubGroupId",
                table: "TypeProducts",
                column: "SubGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_TypeProducts_TypeProducts_SubGroupId",
                table: "TypeProducts",
                column: "SubGroupId",
                principalTable: "TypeProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TypeProducts_TypeProducts_SubGroupId",
                table: "TypeProducts");

            migrationBuilder.DropIndex(
                name: "IX_TypeProducts_SubGroupId",
                table: "TypeProducts");

            migrationBuilder.DeleteData(
                table: "TypeProducts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "IsGroup",
                table: "TypeProducts");

            migrationBuilder.DropColumn(
                name: "SubGroupId",
                table: "TypeProducts");

            migrationBuilder.AddColumn<int>(
                name: "GroupTypeProductId",
                table: "TypeProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "GroupTypeProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubGroupId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupTypeProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupTypeProducts_GroupTypeProducts_SubGroupId",
                        column: x => x.SubGroupId,
                        principalTable: "GroupTypeProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TypeProducts_GroupTypeProductId",
                table: "TypeProducts",
                column: "GroupTypeProductId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupTypeProducts_SubGroupId",
                table: "GroupTypeProducts",
                column: "SubGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_TypeProducts_GroupTypeProducts_GroupTypeProductId",
                table: "TypeProducts",
                column: "GroupTypeProductId",
                principalTable: "GroupTypeProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

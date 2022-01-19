using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetailTrade.EntityFramework.Migrations
{
    public partial class TypeProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GroupTypeProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubGroupId = table.Column<int>(type: "int", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "TypeProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GroupTypeProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TypeProducts_GroupTypeProducts_GroupTypeProductId",
                        column: x => x.GroupTypeProductId,
                        principalTable: "GroupTypeProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupTypeProducts_SubGroupId",
                table: "GroupTypeProducts",
                column: "SubGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_TypeProducts_GroupTypeProductId",
                table: "TypeProducts",
                column: "GroupTypeProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TypeProducts");

            migrationBuilder.DropTable(
                name: "GroupTypeProducts");
        }
    }
}

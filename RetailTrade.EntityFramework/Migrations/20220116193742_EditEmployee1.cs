using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetailTrade.EntityFramework.Migrations
{
    public partial class EditEmployee1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupsEmployees_GroupsEmployees_SubGroupId",
                table: "GroupsEmployees");

            migrationBuilder.InsertData(
                table: "GroupsEmployees",
                columns: new[] { "Id", "Name", "SubGroupId" },
                values: new object[] { 1, "Сотрудники", null });

            migrationBuilder.AddForeignKey(
                name: "FK_GroupsEmployees_GroupsEmployees_SubGroupId",
                table: "GroupsEmployees",
                column: "SubGroupId",
                principalTable: "GroupsEmployees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupsEmployees_GroupsEmployees_SubGroupId",
                table: "GroupsEmployees");

            migrationBuilder.DeleteData(
                table: "GroupsEmployees",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupsEmployees_GroupsEmployees_SubGroupId",
                table: "GroupsEmployees",
                column: "SubGroupId",
                principalTable: "GroupsEmployees",
                principalColumn: "Id");
        }
    }
}

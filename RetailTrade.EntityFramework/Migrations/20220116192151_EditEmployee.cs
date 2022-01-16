using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetailTrade.EntityFramework.Migrations
{
    public partial class EditEmployee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Employees");

            migrationBuilder.AddColumn<int>(
                name: "GroupEmployeeId",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "GroupsEmployees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubGroupId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupsEmployees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupsEmployees_GroupsEmployees_SubGroupId",
                        column: x => x.SubGroupId,
                        principalTable: "GroupsEmployees",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_GroupEmployeeId",
                table: "Employees",
                column: "GroupEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupsEmployees_SubGroupId",
                table: "GroupsEmployees",
                column: "SubGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_GroupsEmployees_GroupEmployeeId",
                table: "Employees",
                column: "GroupEmployeeId",
                principalTable: "GroupsEmployees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_GroupsEmployees_GroupEmployeeId",
                table: "Employees");

            migrationBuilder.DropTable(
                name: "GroupsEmployees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_GroupEmployeeId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "GroupEmployeeId",
                table: "Employees");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

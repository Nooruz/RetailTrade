using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetailTrade.EntityFramework.Migrations
{
    public partial class DocumentNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Number",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Number",
                table: "Documents");
        }
    }
}

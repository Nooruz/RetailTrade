using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetailTrade.EntityFramework.Migrations
{
    public partial class Contractor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContractorTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractorTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contractors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TIN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OKPO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContractorTypeId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Patronymic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Birthdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GenderId = table.Column<int>(type: "int", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contractors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contractors_ContractorTypes_ContractorTypeId",
                        column: x => x.ContractorTypeId,
                        principalTable: "ContractorTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Contractors_Genders_GenderId",
                        column: x => x.GenderId,
                        principalTable: "Genders",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "ContractorTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Юридическое лицо" });

            migrationBuilder.InsertData(
                table: "ContractorTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "Физическое лицо" });

            migrationBuilder.InsertData(
                table: "Contractors",
                columns: new[] { "Id", "Comment", "ContractorTypeId", "Created", "Discriminator", "Email", "FullName", "OKPO", "Phone", "TIN", "WorkName" },
                values: new object[] { 1, null, 2, new DateTime(1995, 3, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Contractor", null, "Розничный покупатель", null, null, null, "Розничный покупатель" });

            migrationBuilder.CreateIndex(
                name: "IX_Contractors_ContractorTypeId",
                table: "Contractors",
                column: "ContractorTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Contractors_GenderId",
                table: "Contractors",
                column: "GenderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contractors");

            migrationBuilder.DropTable(
                name: "ContractorTypes");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetailTrade.EntityFramework.Migrations
{
    public partial class EnterProductToEnterProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EnterProduct_Documents_DocumentId",
                table: "EnterProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_EnterProduct_Products_ProductId",
                table: "EnterProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_EnterProduct_Registrations_RegistrationId",
                table: "EnterProduct");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EnterProduct",
                table: "EnterProduct");

            migrationBuilder.RenameTable(
                name: "EnterProduct",
                newName: "EnterProducts");

            migrationBuilder.RenameIndex(
                name: "IX_EnterProduct_RegistrationId",
                table: "EnterProducts",
                newName: "IX_EnterProducts_RegistrationId");

            migrationBuilder.RenameIndex(
                name: "IX_EnterProduct_ProductId",
                table: "EnterProducts",
                newName: "IX_EnterProducts_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_EnterProduct_DocumentId",
                table: "EnterProducts",
                newName: "IX_EnterProducts_DocumentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EnterProducts",
                table: "EnterProducts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EnterProducts_Documents_DocumentId",
                table: "EnterProducts",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EnterProducts_Products_ProductId",
                table: "EnterProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EnterProducts_Registrations_RegistrationId",
                table: "EnterProducts",
                column: "RegistrationId",
                principalTable: "Registrations",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EnterProducts_Documents_DocumentId",
                table: "EnterProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_EnterProducts_Products_ProductId",
                table: "EnterProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_EnterProducts_Registrations_RegistrationId",
                table: "EnterProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EnterProducts",
                table: "EnterProducts");

            migrationBuilder.RenameTable(
                name: "EnterProducts",
                newName: "EnterProduct");

            migrationBuilder.RenameIndex(
                name: "IX_EnterProducts_RegistrationId",
                table: "EnterProduct",
                newName: "IX_EnterProduct_RegistrationId");

            migrationBuilder.RenameIndex(
                name: "IX_EnterProducts_ProductId",
                table: "EnterProduct",
                newName: "IX_EnterProduct_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_EnterProducts_DocumentId",
                table: "EnterProduct",
                newName: "IX_EnterProduct_DocumentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EnterProduct",
                table: "EnterProduct",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EnterProduct_Documents_DocumentId",
                table: "EnterProduct",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EnterProduct_Products_ProductId",
                table: "EnterProduct",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EnterProduct_Registrations_RegistrationId",
                table: "EnterProduct",
                column: "RegistrationId",
                principalTable: "Registrations",
                principalColumn: "Id");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace RetailTrade.EntityFramework.Migrations
{
    public partial class UpdateOrganization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_ActivityCategories_ActivityCategoryId",
                table: "Organizations");

            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_Users_UserId",
                table: "Organizations");

            migrationBuilder.DropTable(
                name: "ActivityCategories");

            migrationBuilder.DropIndex(
                name: "IX_Organizations_ActivityCategoryId",
                table: "Organizations");

            migrationBuilder.DropIndex(
                name: "IX_Organizations_UserId",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "IsConnected",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ActivityCategoryId",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Organizations");

            migrationBuilder.RenameColumn(
                name: "Surname",
                table: "Organizations",
                newName: "Inn");

            migrationBuilder.RenameColumn(
                name: "OrganizationName",
                table: "Organizations",
                newName: "FullName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Inn",
                table: "Organizations",
                newName: "Surname");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "Organizations",
                newName: "OrganizationName");

            migrationBuilder.AddColumn<bool>(
                name: "IsConnected",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ActivityCategoryId",
                table: "Organizations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Organizations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ActivityCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityCategories", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ActivityCategories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Автосервис / СТО" },
                    { 26, "Ритуальные услуги" },
                    { 25, "Деловые / юридические услуги" },
                    { 24, "Транспортные услуги" },
                    { 23, "Строительные и отделочные работы" },
                    { 22, "Полиграфия и производство рекламных материалов" },
                    { 21, "Окна и балконы" },
                    { 20, "Кондиционирование, вентиляция и отопление" },
                    { 19, "Ветеринарные клиники" },
                    { 18, "Медицинские центры" },
                    { 17, "Стоматология" },
                    { 16, "Салоны красоты и SPA" },
                    { 15, "Розничные и оптовые продажи" },
                    { 14, "Ремонт промышленного оборудования" },
                    { 13, "Ремонт бензо / электроинструмента" },
                    { 12, "Ремонт бытовой техники" },
                    { 11, "Ремонт техники Apple" },
                    { 10, "Ремонт компьютеров / цифровой техники" },
                    { 9, "Ремонт мобильных / смартфонов" },
                    { 8, "Ремонт обуви" },
                    { 7, "Ремонт вело / спортивного спорядження" },
                    { 6, "Химчистки, прачечные, клининг" },
                    { 5, "Ателье по пошиву / ремонту одежды" },
                    { 4, "Аренда и прокат" },
                    { 3, "Автомойка / Шиномонтаж" },
                    { 2, "Детейлинг / Тюнинг" },
                    { 27, "Производство" },
                    { 28, "Другие услуги" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_ActivityCategoryId",
                table: "Organizations",
                column: "ActivityCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_UserId",
                table: "Organizations",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_ActivityCategories_ActivityCategoryId",
                table: "Organizations",
                column: "ActivityCategoryId",
                principalTable: "ActivityCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_Users_UserId",
                table: "Organizations",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

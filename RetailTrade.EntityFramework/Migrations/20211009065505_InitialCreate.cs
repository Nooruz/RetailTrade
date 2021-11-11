using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RetailTrade.EntityFramework.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "ProductCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShortName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Inn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Units",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LongName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShortName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductSubcategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductCategoryId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSubcategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductSubcategories_ProductCategories_ProductCategoryId",
                        column: x => x.ProductCategoryId,
                        principalTable: "ProductCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JoinedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsConnected = table.Column<bool>(type: "bit", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductSubcategoryId = table.Column<int>(type: "int", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    UnitId = table.Column<int>(type: "int", nullable: false),
                    TNVED = table.Column<int>(type: "int", nullable: false),
                    Barcode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ArrivalPrice = table.Column<double>(type: "float", nullable: false),
                    Quantity = table.Column<double>(type: "float", nullable: false),
                    SalePrice = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_ProductSubcategories_ProductSubcategoryId",
                        column: x => x.ProductSubcategoryId,
                        principalTable: "ProductSubcategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganizationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ActivityCategoryId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Organizations_ActivityCategories_ActivityCategoryId",
                        column: x => x.ActivityCategoryId,
                        principalTable: "ActivityCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Organizations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ActivityCategories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Автосервис / СТО" },
                    { 28, "Другие услуги" },
                    { 27, "Производство" },
                    { 26, "Ритуальные услуги" },
                    { 25, "Деловые / юридические услуги" },
                    { 23, "Строительные и отделочные работы" },
                    { 22, "Полиграфия и производство рекламных материалов" },
                    { 21, "Окна и балконы" },
                    { 20, "Кондиционирование, вентиляция и отопление" },
                    { 19, "Ветеринарные клиники" },
                    { 18, "Медицинские центры" },
                    { 17, "Стоматология" },
                    { 16, "Салоны красоты и SPA" },
                    { 15, "Розничные и оптовые продажи" },
                    { 24, "Транспортные услуги" },
                    { 13, "Ремонт бензо / электроинструмента" },
                    { 14, "Ремонт промышленного оборудования" },
                    { 2, "Детейлинг / Тюнинг" },
                    { 3, "Автомойка / Шиномонтаж" },
                    { 4, "Аренда и прокат" },
                    { 6, "Химчистки, прачечные, клининг" },
                    { 5, "Ателье по пошиву / ремонту одежды" },
                    { 8, "Ремонт обуви" },
                    { 9, "Ремонт мобильных / смартфонов" },
                    { 10, "Ремонт компьютеров / цифровой техники" },
                    { 11, "Ремонт техники Apple" },
                    { 12, "Ремонт бытовой техники" },
                    { 7, "Ремонт вело / спортивного спорядження" }
                });

            migrationBuilder.InsertData(
                table: "ProductCategories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 13, "Прочие" },
                    { 9, "Сладости" },
                    { 12, "Чай, кофе" },
                    { 11, "Хлебо-булочные издели" },
                    { 10, "Снэки" },
                    { 8, "Сигареты" },
                    { 4, "Колбасы" },
                    { 6, "Молочные продукты" },
                    { 5, "Консервы" },
                    { 3, "Бытовая химия" },
                    { 2, "Бакалея" },
                    { 1, "Алкоголь" },
                    { 7, "Напитки" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Администратор" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "Продавец" });

            migrationBuilder.InsertData(
                table: "Units",
                columns: new[] { "Id", "LongName", "ShortName" },
                values: new object[,]
                {
                    { 4, "Метр", "м" },
                    { 1, "Килограмм", "кг" },
                    { 2, "Грамм", "г" },
                    { 3, "Литр", "л" },
                    { 5, "Штука", "шт" }
                });

            migrationBuilder.InsertData(
                table: "ProductSubcategories",
                columns: new[] { "Id", "Name", "ProductCategoryId" },
                values: new object[,]
                {
                    { 1, "Водка", 1 },
                    { 24, "Творог", 6 },
                    { 25, "Минеральная вода", 7 },
                    { 26, "Соки", 7 },
                    { 27, "Кент", 8 },
                    { 28, "Бонд", 8 },
                    { 29, "Винстон", 8 },
                    { 30, "Боксы", 9 },
                    { 31, "Вафли", 9 },
                    { 23, "Сыр", 6 },
                    { 32, "Конфеты", 9 },
                    { 34, "Пряники", 9 },
                    { 35, "Шоколад", 9 },
                    { 36, "Сухарики", 10 },
                    { 37, "Фисташки", 10 },
                    { 38, "Чипсы", 10 },
                    { 39, "Сдоба", 11 },
                    { 40, "Хлеб", 11 },
                    { 41, "Кофе", 12 },
                    { 33, "Печенье", 9 },
                    { 42, "Чай", 12 },
                    { 22, "Мороженое", 6 },
                    { 20, "Масло сливочное", 6 },
                    { 2, "Пиво", 1 },
                    { 3, "Крупы", 2 },
                    { 4, "Макаронные изделия", 2 },
                    { 5, "Масло подсолнечное", 2 },
                    { 6, "Мука", 2 },
                    { 7, "Салаты", 2 },
                    { 8, "Мыло", 3 },
                    { 9, "Освежитель воздуха", 3 },
                    { 21, "Молоко", 6 },
                    { 10, "Бекон", 4 },
                    { 12, "Колбаса копченная", 4 },
                    { 13, "Сосиски", 4 },
                    { 14, "Сыры", 4 },
                    { 15, "Грибы", 5 },
                    { 16, "Маслины, оливки", 5 },
                    { 17, "Овощные консервы", 5 },
                    { 18, "Рыбные консервы", 5 },
                    { 19, "Кефир и йогурт", 6 },
                    { 11, "Колбаса вареная", 4 }
                });

            migrationBuilder.InsertData(
                table: "ProductSubcategories",
                columns: new[] { "Id", "Name", "ProductCategoryId" },
                values: new object[] { 43, "Цукерка", 12 });

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_ActivityCategoryId",
                table: "Organizations",
                column: "ActivityCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_UserId",
                table: "Organizations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductSubcategoryId",
                table: "Products",
                column: "ProductSubcategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_SupplierId",
                table: "Products",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_UnitId",
                table: "Products",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSubcategories_ProductCategoryId",
                table: "ProductSubcategories",
                column: "ProductCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Organizations");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "ActivityCategories");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "ProductSubcategories");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "Units");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "ProductCategories");
        }
    }
}

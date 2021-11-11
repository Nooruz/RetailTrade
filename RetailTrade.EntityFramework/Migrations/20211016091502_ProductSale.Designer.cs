﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RetailTrade.EntityFramework;

namespace RetailTrade.EntityFramework.Migrations
{
    [DbContext(typeof(RetailTradeDbContext))]
    [Migration("20211016091502_ProductSale")]
    partial class ProductSale
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.11")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("RetailTrade.Domain.Models.ActivityCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ActivityCategories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Автосервис / СТО"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Детейлинг / Тюнинг"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Автомойка / Шиномонтаж"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Аренда и прокат"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Ателье по пошиву / ремонту одежды"
                        },
                        new
                        {
                            Id = 6,
                            Name = "Химчистки, прачечные, клининг"
                        },
                        new
                        {
                            Id = 7,
                            Name = "Ремонт вело / спортивного спорядження"
                        },
                        new
                        {
                            Id = 8,
                            Name = "Ремонт обуви"
                        },
                        new
                        {
                            Id = 9,
                            Name = "Ремонт мобильных / смартфонов"
                        },
                        new
                        {
                            Id = 10,
                            Name = "Ремонт компьютеров / цифровой техники"
                        },
                        new
                        {
                            Id = 11,
                            Name = "Ремонт техники Apple"
                        },
                        new
                        {
                            Id = 12,
                            Name = "Ремонт бытовой техники"
                        },
                        new
                        {
                            Id = 13,
                            Name = "Ремонт бензо / электроинструмента"
                        },
                        new
                        {
                            Id = 14,
                            Name = "Ремонт промышленного оборудования"
                        },
                        new
                        {
                            Id = 15,
                            Name = "Розничные и оптовые продажи"
                        },
                        new
                        {
                            Id = 16,
                            Name = "Салоны красоты и SPA"
                        },
                        new
                        {
                            Id = 17,
                            Name = "Стоматология"
                        },
                        new
                        {
                            Id = 18,
                            Name = "Медицинские центры"
                        },
                        new
                        {
                            Id = 19,
                            Name = "Ветеринарные клиники"
                        },
                        new
                        {
                            Id = 20,
                            Name = "Кондиционирование, вентиляция и отопление"
                        },
                        new
                        {
                            Id = 21,
                            Name = "Окна и балконы"
                        },
                        new
                        {
                            Id = 22,
                            Name = "Полиграфия и производство рекламных материалов"
                        },
                        new
                        {
                            Id = 23,
                            Name = "Строительные и отделочные работы"
                        },
                        new
                        {
                            Id = 24,
                            Name = "Транспортные услуги"
                        },
                        new
                        {
                            Id = 25,
                            Name = "Деловые / юридические услуги"
                        },
                        new
                        {
                            Id = 26,
                            Name = "Ритуальные услуги"
                        },
                        new
                        {
                            Id = 27,
                            Name = "Производство"
                        },
                        new
                        {
                            Id = 28,
                            Name = "Другие услуги"
                        });
                });

            modelBuilder.Entity("RetailTrade.Domain.Models.Branch", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Branches");
                });

            modelBuilder.Entity("RetailTrade.Domain.Models.Organization", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ActivityCategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OrganizationName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Surname")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ActivityCategoryId");

                    b.HasIndex("UserId");

                    b.ToTable("Organizations");
                });

            modelBuilder.Entity("RetailTrade.Domain.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("ArrivalPrice")
                        .HasColumnType("float");

                    b.Property<string>("Barcode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProductSubcategoryId")
                        .HasColumnType("int");

                    b.Property<double>("Quantity")
                        .HasColumnType("float");

                    b.Property<double>("SalePrice")
                        .HasColumnType("float");

                    b.Property<int>("SupplierId")
                        .HasColumnType("int");

                    b.Property<int>("TNVED")
                        .HasColumnType("int");

                    b.Property<int>("UnitId")
                        .HasColumnType("int");

                    b.Property<bool>("WithoutBarcode")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("ProductSubcategoryId");

                    b.HasIndex("SupplierId");

                    b.HasIndex("UnitId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("RetailTrade.Domain.Models.ProductCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ProductCategories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Алкоголь"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Бакалея"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Бытовая химия"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Колбасы"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Консервы"
                        },
                        new
                        {
                            Id = 6,
                            Name = "Молочные продукты"
                        },
                        new
                        {
                            Id = 7,
                            Name = "Напитки"
                        },
                        new
                        {
                            Id = 8,
                            Name = "Сигареты"
                        },
                        new
                        {
                            Id = 9,
                            Name = "Сладости"
                        },
                        new
                        {
                            Id = 10,
                            Name = "Снэки"
                        },
                        new
                        {
                            Id = 11,
                            Name = "Хлебо-булочные издели"
                        },
                        new
                        {
                            Id = 12,
                            Name = "Чай, кофе"
                        },
                        new
                        {
                            Id = 13,
                            Name = "Прочие"
                        });
                });

            modelBuilder.Entity("RetailTrade.Domain.Models.ProductSale", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<double>("Quantity")
                        .HasColumnType("float");

                    b.Property<DateTime>("SaleDate")
                        .HasColumnType("datetime2");

                    b.Property<double>("Sum")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductSales");
                });

            modelBuilder.Entity("RetailTrade.Domain.Models.ProductSubcategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProductCategoryId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductCategoryId");

                    b.ToTable("ProductSubcategories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Водка",
                            ProductCategoryId = 1
                        },
                        new
                        {
                            Id = 2,
                            Name = "Пиво",
                            ProductCategoryId = 1
                        },
                        new
                        {
                            Id = 3,
                            Name = "Крупы",
                            ProductCategoryId = 2
                        },
                        new
                        {
                            Id = 4,
                            Name = "Макаронные изделия",
                            ProductCategoryId = 2
                        },
                        new
                        {
                            Id = 5,
                            Name = "Масло подсолнечное",
                            ProductCategoryId = 2
                        },
                        new
                        {
                            Id = 6,
                            Name = "Мука",
                            ProductCategoryId = 2
                        },
                        new
                        {
                            Id = 7,
                            Name = "Салаты",
                            ProductCategoryId = 2
                        },
                        new
                        {
                            Id = 8,
                            Name = "Мыло",
                            ProductCategoryId = 3
                        },
                        new
                        {
                            Id = 9,
                            Name = "Освежитель воздуха",
                            ProductCategoryId = 3
                        },
                        new
                        {
                            Id = 10,
                            Name = "Бекон",
                            ProductCategoryId = 4
                        },
                        new
                        {
                            Id = 11,
                            Name = "Колбаса вареная",
                            ProductCategoryId = 4
                        },
                        new
                        {
                            Id = 12,
                            Name = "Колбаса копченная",
                            ProductCategoryId = 4
                        },
                        new
                        {
                            Id = 13,
                            Name = "Сосиски",
                            ProductCategoryId = 4
                        },
                        new
                        {
                            Id = 14,
                            Name = "Сыры",
                            ProductCategoryId = 4
                        },
                        new
                        {
                            Id = 15,
                            Name = "Грибы",
                            ProductCategoryId = 5
                        },
                        new
                        {
                            Id = 16,
                            Name = "Маслины, оливки",
                            ProductCategoryId = 5
                        },
                        new
                        {
                            Id = 17,
                            Name = "Овощные консервы",
                            ProductCategoryId = 5
                        },
                        new
                        {
                            Id = 18,
                            Name = "Рыбные консервы",
                            ProductCategoryId = 5
                        },
                        new
                        {
                            Id = 19,
                            Name = "Кефир и йогурт",
                            ProductCategoryId = 6
                        },
                        new
                        {
                            Id = 20,
                            Name = "Масло сливочное",
                            ProductCategoryId = 6
                        },
                        new
                        {
                            Id = 21,
                            Name = "Молоко",
                            ProductCategoryId = 6
                        },
                        new
                        {
                            Id = 22,
                            Name = "Мороженое",
                            ProductCategoryId = 6
                        },
                        new
                        {
                            Id = 23,
                            Name = "Сыр",
                            ProductCategoryId = 6
                        },
                        new
                        {
                            Id = 24,
                            Name = "Творог",
                            ProductCategoryId = 6
                        },
                        new
                        {
                            Id = 25,
                            Name = "Минеральная вода",
                            ProductCategoryId = 7
                        },
                        new
                        {
                            Id = 26,
                            Name = "Соки",
                            ProductCategoryId = 7
                        },
                        new
                        {
                            Id = 27,
                            Name = "Кент",
                            ProductCategoryId = 8
                        },
                        new
                        {
                            Id = 28,
                            Name = "Бонд",
                            ProductCategoryId = 8
                        },
                        new
                        {
                            Id = 29,
                            Name = "Винстон",
                            ProductCategoryId = 8
                        },
                        new
                        {
                            Id = 30,
                            Name = "Боксы",
                            ProductCategoryId = 9
                        },
                        new
                        {
                            Id = 31,
                            Name = "Вафли",
                            ProductCategoryId = 9
                        },
                        new
                        {
                            Id = 32,
                            Name = "Конфеты",
                            ProductCategoryId = 9
                        },
                        new
                        {
                            Id = 33,
                            Name = "Печенье",
                            ProductCategoryId = 9
                        },
                        new
                        {
                            Id = 34,
                            Name = "Пряники",
                            ProductCategoryId = 9
                        },
                        new
                        {
                            Id = 35,
                            Name = "Шоколад",
                            ProductCategoryId = 9
                        },
                        new
                        {
                            Id = 36,
                            Name = "Сухарики",
                            ProductCategoryId = 10
                        },
                        new
                        {
                            Id = 37,
                            Name = "Фисташки",
                            ProductCategoryId = 10
                        },
                        new
                        {
                            Id = 38,
                            Name = "Чипсы",
                            ProductCategoryId = 10
                        },
                        new
                        {
                            Id = 39,
                            Name = "Сдоба",
                            ProductCategoryId = 11
                        },
                        new
                        {
                            Id = 40,
                            Name = "Хлеб",
                            ProductCategoryId = 11
                        },
                        new
                        {
                            Id = 41,
                            Name = "Кофе",
                            ProductCategoryId = 12
                        },
                        new
                        {
                            Id = 42,
                            Name = "Чай",
                            ProductCategoryId = 12
                        },
                        new
                        {
                            Id = 43,
                            Name = "Цукерка",
                            ProductCategoryId = 12
                        });
                });

            modelBuilder.Entity("RetailTrade.Domain.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Администратор"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Кассир"
                        });
                });

            modelBuilder.Entity("RetailTrade.Domain.Models.Supplier", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Inn")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ShortName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Suppliers");
                });

            modelBuilder.Entity("RetailTrade.Domain.Models.Unit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("LongName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ShortName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Units");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            LongName = "Килограмм",
                            ShortName = "кг"
                        },
                        new
                        {
                            Id = 2,
                            LongName = "Грамм",
                            ShortName = "г"
                        },
                        new
                        {
                            Id = 3,
                            LongName = "Литр",
                            ShortName = "л"
                        },
                        new
                        {
                            Id = 4,
                            LongName = "Метр",
                            ShortName = "м"
                        },
                        new
                        {
                            Id = 5,
                            LongName = "Штука",
                            ShortName = "шт"
                        });
                });

            modelBuilder.Entity("RetailTrade.Domain.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsConnected")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("JoinedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("RetailTrade.Domain.Models.Branch", b =>
                {
                    b.HasOne("RetailTrade.Domain.Models.User", "User")
                        .WithOne("Branch")
                        .HasForeignKey("RetailTrade.Domain.Models.Branch", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("RetailTrade.Domain.Models.Organization", b =>
                {
                    b.HasOne("RetailTrade.Domain.Models.ActivityCategory", "ActivityCategory")
                        .WithMany("Organizations")
                        .HasForeignKey("ActivityCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RetailTrade.Domain.Models.User", "User")
                        .WithMany("Organizations")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ActivityCategory");

                    b.Navigation("User");
                });

            modelBuilder.Entity("RetailTrade.Domain.Models.Product", b =>
                {
                    b.HasOne("RetailTrade.Domain.Models.ProductSubcategory", "ProductSubcategory")
                        .WithMany("Products")
                        .HasForeignKey("ProductSubcategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RetailTrade.Domain.Models.Supplier", "Supplier")
                        .WithMany("Products")
                        .HasForeignKey("SupplierId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RetailTrade.Domain.Models.Unit", "Unit")
                        .WithMany()
                        .HasForeignKey("UnitId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ProductSubcategory");

                    b.Navigation("Supplier");

                    b.Navigation("Unit");
                });

            modelBuilder.Entity("RetailTrade.Domain.Models.ProductSale", b =>
                {
                    b.HasOne("RetailTrade.Domain.Models.Product", "Product")
                        .WithMany("ProductSales")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("RetailTrade.Domain.Models.ProductSubcategory", b =>
                {
                    b.HasOne("RetailTrade.Domain.Models.ProductCategory", "ProductCategory")
                        .WithMany("ProductSubcategories")
                        .HasForeignKey("ProductCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ProductCategory");
                });

            modelBuilder.Entity("RetailTrade.Domain.Models.User", b =>
                {
                    b.HasOne("RetailTrade.Domain.Models.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("RetailTrade.Domain.Models.ActivityCategory", b =>
                {
                    b.Navigation("Organizations");
                });

            modelBuilder.Entity("RetailTrade.Domain.Models.Product", b =>
                {
                    b.Navigation("ProductSales");
                });

            modelBuilder.Entity("RetailTrade.Domain.Models.ProductCategory", b =>
                {
                    b.Navigation("ProductSubcategories");
                });

            modelBuilder.Entity("RetailTrade.Domain.Models.ProductSubcategory", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("RetailTrade.Domain.Models.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("RetailTrade.Domain.Models.Supplier", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("RetailTrade.Domain.Models.User", b =>
                {
                    b.Navigation("Branch");

                    b.Navigation("Organizations");
                });
#pragma warning restore 612, 618
        }
    }
}

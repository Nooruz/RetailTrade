using Microsoft.EntityFrameworkCore;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Views;
using System;
using System.Linq;

namespace RetailTrade.EntityFramework
{
    public class RetailTradeDbContext : DbContext
    {
        #region Domains

        /// <summary>
        /// Организация
        /// </summary>
        public DbSet<Organization> Organizations { get; set; }

        /// <summary>
        /// Роли пользователей
        /// </summary>
        public DbSet<Role> Roles { get; set; }

        /// <summary>
        /// Пользователи
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Единица измерения
        /// </summary>
        public DbSet<Unit> Units { get; set; }

        /// <summary>
        /// Товары
        /// </summary>
        public DbSet<Product> Products { get; set; }

        /// <summary>
        /// Поставщики
        /// </summary>
        public DbSet<Supplier> Suppliers { get; set; }

        /// <summary>
        /// Филиалы
        /// </summary>
        public DbSet<Branch> Branches { get; set; }

        /// <summary>
        /// Продажа товаров
        /// </summary>
        public DbSet<ProductSale> ProductSales { get; set; }

        /// <summary>
        /// Чеки
        /// </summary>
        public DbSet<Receipt> Receipts { get; set; }

        /// <summary>
        /// Смены
        /// </summary>
        public DbSet<Shift> Shifts { get; set; }

        public DbSet<Arrival> Arrivals { get; set; }

        /// <summary>
        /// Приход товаров
        /// </summary>
        public DbSet<ArrivalProduct> ArrivalProducts { get; set; }

        public DbSet<WriteDown> WriteDowns { get; set; }

        /// <summary>
        /// Товары возврашенные поставшику
        /// </summary>
        public DbSet<RefundToSupplierProduct> RefundToSupplierProducts { get; set; }

        public DbSet<RefundToSupplier> RefundsToSuppliers { get; set; }

        /// <summary>
        /// Статусы заказа
        /// </summary>
        public DbSet<OrderStatus> OrderStatuses { get; set; }

        /// <summary>
        /// Заказанные товары
        /// </summary>
        public DbSet<OrderProduct> OrderProducts { get; set; }

        /// <summary>
        /// Заказы поставщиков
        /// </summary>
        public DbSet<OrderToSupplier> OrdersToSuppliers { get; set; }

        /// <summary>
        /// Возвраты
        /// </summary>
        public DbSet<Refund> Refunds { get; set; }

        /// <summary>
        /// Возврат товары
        /// </summary>
        public DbSet<ProductRefund> ProductRefunds { get; set; }

        /// <summary>
        /// Тип шаблона ценников и этикеток
        /// </summary>
        public DbSet<TypeLabelPriceTag> TypeLabelPriceTags { get; set; }

        /// <summary>
        /// Шаблон ценников и этикеток
        /// </summary>
        public DbSet<LabelPriceTag> LabelPriceTags { get; set; }

        /// <summary>
        /// Размеры ценников и этикеток
        /// </summary>
        public DbSet<LabelPriceTagSize> LabelPriceTagSizes { get; set; }
        public DbSet<ContractorType> ContractorTypes { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Contractor> Contractors { get; set; }
        public DbSet<PointSale> PointSales { get; set; }
        public DbSet<UserPointSale> UserPointSales { get; set; }

        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentProduct> DocumentProducts { get; set; }

        #endregion

        #region Employee

        /// <summary>
        /// Гендеры
        /// </summary>
        public DbSet<Gender> Genders { get; set; }

        /// <summary>
        /// Сотрудники
        /// </summary>
        public DbSet<Employee> Employees { get; set; }

        /// <summary>
        /// Группа сотрудников
        /// </summary>
        public DbSet<GroupEmployee> GroupsEmployees { get; set; }

        #endregion

        #region Product

        public DbSet<TypeProduct> TypeProducts { get; set; }
        public DbSet<ProductBarcode> ProductBarcode { get; set; }
        
        #endregion

        #region Revaluation

        public DbSet<Revaluation> Revaluations { get; set; }
        public DbSet<RevaluationProduct> RevaluationProducts { get; set; }

        #endregion

        #region WareHouse

        public DbSet<WareHouse> WareHouses { get; set; }
        public DbSet<TypeWareHouse> TypeWareHouses { get; set; }

        #endregion

        #region Views

        public DbSet<ProductWareHouseView> ProductWareHouseViews { get; set; }
        public DbSet<ProductView> ProductViews { get; set; }
        public DbSet<ProductBarcodeView> ProductBarcodeViews { get; set; }
        public DbSet<ReceiptView> ReceiptViews { get; set; }
        public DbSet<ProductSaleView> ProductSaleViews { get; set; }
        public DbSet<DocumentView> DocumentViews { get; set; }
        public DbSet<DocumentProductView> EnterDocumentViews { get; set; }
        public DbSet<ProductStockView> ProductStockViews { get; set; }
        public DbSet<ProductIncomingHistoryView> ProductIncomingHistoryViews { get; set; }
        public DbSet<ProductOutcomingHistoryView> ProductOutcomingHistoryViews { get; set; }

        #endregion

        public IQueryable<ProductView> ProductFunction(int wareHouseId) => FromExpression(() => ProductFunction(wareHouseId));
        public IQueryable<WareHouseView> WareHouseFunction(int productId) => FromExpression(() => WareHouseFunction(productId));

        public RetailTradeDbContext(DbContextOptions<RetailTradeDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            _ = modelBuilder.Entity<ProductOutcomingHistoryView>()
                .ToView(nameof(ProductOutcomingHistoryView))
                .HasNoKey();

            _ = modelBuilder.Entity<ProductIncomingHistoryView>()
                .ToView(nameof(ProductIncomingHistoryView))
                .HasNoKey();

            _ = modelBuilder.Entity<ProductStockView>()
                .ToView(nameof(ProductStockView))
                .HasNoKey();

            _ = modelBuilder.Entity<DocumentProductView>()
                .ToView(nameof(DocumentProductView))
                .HasNoKey();

            _ = modelBuilder.Entity<DocumentView>()
                .ToView(nameof(DocumentView))
                .HasNoKey();

            _ = modelBuilder.Entity<DocumentType>().HasData(
                new DocumentType { Id = 1, Name = "Приемка" },
                new DocumentType { Id = 2, Name = "Возврат поставщику" },
                new DocumentType { Id = 3, Name = "Списание" },
                new DocumentType { Id = 4, Name = "Оприходование" },
                new DocumentType { Id = 5, Name = "Перемещение" },
                new DocumentType { Id = 6, Name = "Инвентаризация" },
                new DocumentType { Id = 7, Name = "Приходный ордер" },
                new DocumentType { Id = 8, Name = "Расходный ордер" },
                new DocumentType { Id = 9, Name = "Корректировка остатков в кассе" },
                new DocumentType { Id = 10, Name = "Корректировка остатков на счете" },
                new DocumentType { Id = 11, Name = "Продажа" },
                new DocumentType { Id = 12, Name = "Возврат" },
                new DocumentType { Id = 13, Name = "Смена" },
                new DocumentType { Id = 14, Name = "Внесение" },
                new DocumentType { Id = 15, Name = "Выплата" });

            modelBuilder.HasDbFunction(() => WareHouseFunction(default));
            modelBuilder.HasDbFunction(() => ProductFunction(default));

            modelBuilder.Entity<PointSale>()
                .HasMany(p => p.Users)
                .WithMany(u => u.PointSales)
                .UsingEntity<UserPointSale>(
                j => j
                    .HasOne(pt => pt.User)
                    .WithMany(p => p.UserPointSales)
                    .HasForeignKey(pt => pt.UserId),
                j => j
                    .HasOne(pt => pt.PointSale)
                    .WithMany(p => p.UserPointSale)
                    .HasForeignKey(pt => pt.PointSaleId),
                j =>
                    {
                        j.HasKey(k => new { k.PointSaleId, k.UserId });
                        j.ToTable("UserPointSales");
                    });

            modelBuilder.Entity<ProductSaleView>()
                .ToView(nameof(ProductSaleView))
                .HasNoKey();

            modelBuilder.Entity<ReceiptView>()
                .ToView(nameof(ReceiptView))
                .HasNoKey();

            modelBuilder.Entity<ProductBarcodeView>()
                .ToView(nameof(ProductBarcodeView))
                .HasNoKey();

            modelBuilder.Entity<ProductView>()
                .ToView(nameof(ProductView))
                .HasNoKey();

            modelBuilder.Entity<ProductWareHouseView>()
                .ToView(nameof(ProductWareHouseView))
                .HasNoKey();

            modelBuilder.Entity<OrderToSupplier>()
                .HasMany(o => o.OrderProducts)
                .WithOne(o => o.OrderToSupplier)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Arrival>()
                .HasMany(o => o.ArrivalProducts)
                .WithOne(o => o.Arrival)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RefundToSupplier>()
                .HasMany(o => o.RefundToSupplierProducts)
                .WithOne(o => o.RefundToSupplier)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Refund>()
                .HasMany(o => o.ProductRefunds)
                .WithOne(o => o.Refund)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GroupEmployee>(entity =>
            {
                entity.HasKey(g => g.Id);
                entity.Property(g => g.Name);
                entity.HasOne(g => g.SubGroup)
                    .WithMany(g => g.SubGroups)
                    .HasForeignKey(g => g.SubGroupId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            _ = modelBuilder.Entity<WareHouse>().HasData(
                new WareHouse { Id = 1, Name = "Основной склад" });

            modelBuilder.Entity<Gender>().HasData(
                new Gender { Id = 1, Name = "Мужской" },
                new Gender { Id = 2, Name = "Женский" });

            modelBuilder.Entity<ContractorType>().HasData(
                new ContractorType { Id = 1, Name = "Юридическое лицо" },
                new ContractorType { Id = 2, Name = "Физическое лицо" });

            modelBuilder.Entity<Contractor>().HasData(
                new Contractor { Id = 1, FullName = "Розничный покупатель", WorkName = "Розничный покупатель", Created = new DateTime(1995,3,21), ContractorTypeId = 2 });

            modelBuilder.Entity<TypeProduct>(entity =>
            {
                entity.HasKey(g => g.Id);
                entity.Property(g => g.Name);
                entity.HasOne(g => g.SubGroup)
                    .WithMany(g => g.SubGroups)
                    .HasForeignKey(g => g.SubGroupId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<TypeLabelPriceTag>().HasData(
                new TypeLabelPriceTag { Id = 1, Name = "Этикетка для товаров" },
                new TypeLabelPriceTag { Id = 2, Name = "Ценник для товаров" });

            modelBuilder.Entity<LabelPriceTagSize>().HasData(
                new LabelPriceTagSize { Id = 1, Height = 250, Width = 460, TypeLabelPriceTagId = 1 });

            modelBuilder.Entity<LabelPriceTag>().HasData(
                new LabelPriceTag { Id = 1, Name = "Этикетка для товара", TypeLabelPriceTagId = 1, LabelPriceTagSizeId = 1 },
                new LabelPriceTag { Id = 2, Name = "Ценник для товара", TypeLabelPriceTagId = 2 });

            modelBuilder.Entity<Unit>().HasData(
                new Unit { Id = 1, LongName = "Килограмм", ShortName = "кг" },
                new Unit { Id = 2, LongName = "Грамм", ShortName = "г" },
                new Unit { Id = 3, LongName = "Литр", ShortName = "л" },
                new Unit { Id = 4, LongName = "Метр", ShortName = "м" },
                new Unit { Id = 5, LongName = "Штука", ShortName = "шт" });

            modelBuilder.Entity<OrderStatus>().HasData(
                new OrderStatus { Id = 1, Name = "Ожидается поступление" },
                new OrderStatus { Id = 2, Name = "Закрыт" },
                new OrderStatus { Id = 3, Name = "Не выполнено" });

            modelBuilder.Entity<GroupEmployee>().HasData(
                new GroupEmployee { Id = 1, Name = "Сотрудники" },
                new GroupEmployee { Id = 2, Name = "Руководство", SubGroupId = 1 },
                new GroupEmployee { Id = 3, Name = "Кассиры", SubGroupId = 1 });

            modelBuilder.Entity<TypeProduct>().HasData(
                new TypeProduct { Id = 1, Name = "Виды товаров", IsGroup = true });

            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Администратор"},
                new Role { Id = 2, Name = "Кассир" });

            modelBuilder.Entity<TypeWareHouse>().HasData(
                new TypeWareHouse { Id = 1, Name = "Склад" },
                new TypeWareHouse { Id = 2, Name = "Розничный магазин" });

            base.OnModelCreating(modelBuilder);
        }
    }
}

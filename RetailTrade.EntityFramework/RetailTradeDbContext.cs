using Microsoft.EntityFrameworkCore;
using RetailTrade.Domain.Models;

namespace RetailTrade.EntityFramework
{
    public class RetailTradeDbContext : DbContext
    {
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
        /// Группа товаров
        /// </summary>
        public DbSet<ProductCategory> ProductCategories { get; set; }

        /// <summary>
        /// Категория товаров
        /// </summary>
        public DbSet<ProductSubcategory> ProductSubcategories { get; set; }

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

        /// <summary>
        /// Списания товаров
        /// </summary>
        public DbSet<WriteDownProduct> WriteDownProducts { get; set; }

        /// <summary>
        /// Товары возврашенные поставшику
        /// </summary>
        public DbSet<ProductRefundToSupplier> ProductRefunds { get; set; }

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

        public RetailTradeDbContext(DbContextOptions<RetailTradeDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderToSupplier>()
                .HasMany(o => o.OrderProducts)
                .WithOne(o => o.OrderToSupplier)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Arrival>()
                .HasMany(o => o.ArrivalProducts)
                .WithOne(o => o.Arrival)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Unit>().HasData(
                new Unit { Id = 1, LongName = "Килограмм", ShortName = "кг" },
                new Unit { Id = 2, LongName = "Грамм", ShortName = "г" },
                new Unit { Id = 3, LongName = "Литр", ShortName = "л" },
                new Unit { Id = 4, LongName = "Метр", ShortName = "м" },
                new Unit { Id = 5, LongName = "Штука", ShortName = "шт" }
                );

            modelBuilder.Entity<OrderStatus>().HasData(
                new OrderStatus { Id = 1, Name = "Ожидание" },
                new OrderStatus { Id = 2, Name = "Выполнено" },
                new OrderStatus { Id = 3, Name = "Не выполнено" }
                );

            modelBuilder.Entity<ProductCategory>().HasData(
                new ProductCategory { Id = 1, Name = "Алкоголь" },
                new ProductCategory { Id = 2, Name = "Бакалея" },
                new ProductCategory { Id = 3, Name = "Бытовая химия" },
                new ProductCategory { Id = 4, Name = "Колбасы" },
                new ProductCategory { Id = 5, Name = "Консервы" },
                new ProductCategory { Id = 6, Name = "Молочные продукты" },
                new ProductCategory { Id = 7, Name = "Напитки" },
                new ProductCategory { Id = 8, Name = "Сигареты" },
                new ProductCategory { Id = 9, Name = "Сладости" },
                new ProductCategory { Id = 10, Name = "Снэки" },
                new ProductCategory { Id = 11, Name = "Хлебо-булочные издели" },
                new ProductCategory { Id = 12, Name = "Чай, кофе" },
                new ProductCategory { Id = 13, Name = "Прочие" }
                );

            modelBuilder.Entity<ProductSubcategory>().HasData(
                new ProductSubcategory { Id = 1, ProductCategoryId = 1, Name = "Водка" },
                new ProductSubcategory { Id = 2, ProductCategoryId = 1, Name = "Пиво" },
                new ProductSubcategory { Id = 3, ProductCategoryId = 2, Name = "Крупы" },
                new ProductSubcategory { Id = 4, ProductCategoryId = 2, Name = "Макаронные изделия" },
                new ProductSubcategory { Id = 5, ProductCategoryId = 2, Name = "Масло подсолнечное" },
                new ProductSubcategory { Id = 6, ProductCategoryId = 2, Name = "Мука" },
                new ProductSubcategory { Id = 7, ProductCategoryId = 2, Name = "Салаты" },
                new ProductSubcategory { Id = 8, ProductCategoryId = 3, Name = "Мыло" },
                new ProductSubcategory { Id = 9, ProductCategoryId = 3, Name = "Освежитель воздуха" },
                new ProductSubcategory { Id = 10, ProductCategoryId = 4, Name = "Бекон" },
                new ProductSubcategory { Id = 11, ProductCategoryId = 4, Name = "Колбаса вареная" },
                new ProductSubcategory { Id = 12, ProductCategoryId = 4, Name = "Колбаса копченная" },
                new ProductSubcategory { Id = 13, ProductCategoryId = 4, Name = "Сосиски" },
                new ProductSubcategory { Id = 14, ProductCategoryId = 4, Name = "Сыры" },
                new ProductSubcategory { Id = 15, ProductCategoryId = 5, Name = "Грибы" },
                new ProductSubcategory { Id = 16, ProductCategoryId = 5, Name = "Маслины, оливки" },
                new ProductSubcategory { Id = 17, ProductCategoryId = 5, Name = "Овощные консервы" },
                new ProductSubcategory { Id = 18, ProductCategoryId = 5, Name = "Рыбные консервы" },
                new ProductSubcategory { Id = 19, ProductCategoryId = 6, Name = "Кефир и йогурт" },
                new ProductSubcategory { Id = 20, ProductCategoryId = 6, Name = "Масло сливочное" },
                new ProductSubcategory { Id = 21, ProductCategoryId = 6, Name = "Молоко" },
                new ProductSubcategory { Id = 22, ProductCategoryId = 6, Name = "Мороженое" },
                new ProductSubcategory { Id = 23, ProductCategoryId = 6, Name = "Сыр" },
                new ProductSubcategory { Id = 24, ProductCategoryId = 6, Name = "Творог" },
                new ProductSubcategory { Id = 25, ProductCategoryId = 7, Name = "Минеральная вода" },
                new ProductSubcategory { Id = 26, ProductCategoryId = 7, Name = "Соки" },
                new ProductSubcategory { Id = 27, ProductCategoryId = 8, Name = "Кент" },
                new ProductSubcategory { Id = 28, ProductCategoryId = 8, Name = "Бонд" },
                new ProductSubcategory { Id = 29, ProductCategoryId = 8, Name = "Винстон" },
                new ProductSubcategory { Id = 30, ProductCategoryId = 9, Name = "Боксы" },
                new ProductSubcategory { Id = 31, ProductCategoryId = 9, Name = "Вафли" },
                new ProductSubcategory { Id = 32, ProductCategoryId = 9, Name = "Конфеты" },
                new ProductSubcategory { Id = 33, ProductCategoryId = 9, Name = "Печенье" },
                new ProductSubcategory { Id = 34, ProductCategoryId = 9, Name = "Пряники" },
                new ProductSubcategory { Id = 35, ProductCategoryId = 9, Name = "Шоколад" },
                new ProductSubcategory { Id = 36, ProductCategoryId = 10, Name = "Сухарики" },
                new ProductSubcategory { Id = 37, ProductCategoryId = 10, Name = "Фисташки" },
                new ProductSubcategory { Id = 38, ProductCategoryId = 10, Name = "Чипсы" },
                new ProductSubcategory { Id = 39, ProductCategoryId = 11, Name = "Сдоба" },
                new ProductSubcategory { Id = 40, ProductCategoryId = 11, Name = "Хлеб" },
                new ProductSubcategory { Id = 41, ProductCategoryId = 12, Name = "Кофе" },
                new ProductSubcategory { Id = 42, ProductCategoryId = 12, Name = "Чай" },
                new ProductSubcategory { Id = 43, ProductCategoryId = 12, Name = "Цукерка" }
                );

            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Администратор"},
                new Role { Id = 2, Name = "Кассир" }
                );

            base.OnModelCreating(modelBuilder);
        }
    }
}

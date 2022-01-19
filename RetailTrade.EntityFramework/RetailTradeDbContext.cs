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

        #endregion

        #region Equipment

        public DbSet<TypeEquipment> TypeEquipments { get; set; }

        #endregion

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

            modelBuilder.Entity<WriteDown>()
                .HasMany(o => o.WriteDownProducts)
                .WithOne(o => o.WriteDown)
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

            modelBuilder.Entity<Unit>().HasData(
                new Unit { Id = 1, LongName = "Килограмм", ShortName = "кг" },
                new Unit { Id = 2, LongName = "Грамм", ShortName = "г" },
                new Unit { Id = 3, LongName = "Литр", ShortName = "л" },
                new Unit { Id = 4, LongName = "Метр", ShortName = "м" },
                new Unit { Id = 5, LongName = "Штука", ShortName = "шт" });

            modelBuilder.Entity<OrderStatus>().HasData(
                new OrderStatus { Id = 1, Name = "Ожидание" },
                new OrderStatus { Id = 2, Name = "Выполнено" },
                new OrderStatus { Id = 3, Name = "Не выполнено" });

            modelBuilder.Entity<Gender>().HasData(
                new Gender { Id = 1, Name = "Мужской" },
                new Gender { Id = 2, Name = "Женский" });

            modelBuilder.Entity<GroupEmployee>().HasData(
                new GroupEmployee { Id = 1, Name = "Сотрудники" },
                new GroupEmployee { Id = 2, Name = "Руководство", SubGroupId = 1 },
                new GroupEmployee { Id = 3, Name = "Кассиры", SubGroupId = 1 });

            modelBuilder.Entity<TypeProduct>().HasData(
                new TypeProduct { Id = 1, Name = "Виды товаров", IsGroup = true });

            modelBuilder.Entity<TypeEquipment>().HasData(
                new TypeEquipment { Id = 1, Name = "Сканеры штрихкода" },
                new TypeEquipment { Id = 2, Name = "Контрольно-кассовая машина (ККМ)" },
                new TypeEquipment { Id = 3, Name = "Принтеры чеков" },
                new TypeEquipment { Id = 4, Name = "Принтеры этикеток" });

            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Администратор"},
                new Role { Id = 2, Name = "Кассир" });

            base.OnModelCreating(modelBuilder);
        }
    }
}

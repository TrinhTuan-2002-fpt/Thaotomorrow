using Microsoft.EntityFrameworkCore;

namespace PerfumeShop.Models
{
    public class DBContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer();
        }

        public DbSet<ProductTypes> ProductTypes { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Fragrant> Fragrant { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Accounts> Accounts { get; set; }
        public DbSet<Carts> Carts { get; set; }
        public DbSet<Shippers> Shippers { get; set; }
        public DbSet<Customers> Customers { get; set; }
        public DbSet<CartDetails> CartDetails { get; set; }
        public DbSet<Address> Address { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductTypes>().HasKey(t => t.TypeId);
            modelBuilder.Entity<Products>().HasKey(t => t.ProdcutId);
            modelBuilder.Entity<Fragrant>().HasKey(t => t.FragrantId);
            modelBuilder.Entity<Roles>().HasKey(t => t.RoleId);
            modelBuilder.Entity<Accounts>().HasKey(t => t.AccountId);
            modelBuilder.Entity<Carts>().HasKey(c => c.CartId);
            modelBuilder.Entity<Shippers>().HasKey(c => c.ShipperId);
            modelBuilder.Entity<Customers>().HasKey(c => c.CustomerId);
            modelBuilder.Entity<Address>().HasKey(c => c.AddressId);

            modelBuilder.Entity<ProductTypes>().HasMany<Products>(c => c.products)
                .WithOne(e => e.ProductType).HasForeignKey(c => c.TypeId);
            modelBuilder.Entity<Fragrant>().HasMany<Products>(c => c.products)
                .WithOne(e => e.Fragrant).HasForeignKey(c => c.FragrantId);
            modelBuilder.Entity<Roles>().HasMany<Accounts>(c => c.Account)
                .WithOne(c => c.Roles).HasForeignKey(c => c.RoleId);
            modelBuilder.Entity<Shippers>().HasMany<Carts>(c => c.Carts)
                .WithOne(c => c.Shippers).HasForeignKey(c => c.ShipperId);
            modelBuilder.Entity<Customers>().HasMany<Carts>(c => c.Carts)
                .WithOne(c => c.Customers).HasForeignKey(c => c.CustomerId);

            modelBuilder.Entity<CartDetails>().HasKey(c => new {c.CartId, c.ProductId});

            modelBuilder.Entity<CartDetails>().HasOne<Carts>(c => c.Carts)
                .WithMany(c => c.CartDetails).HasForeignKey(c => c.CartId);

            modelBuilder.Entity<CartDetails>().HasOne<Products>(c => c.Products)
                .WithMany(c => c.CartDetails).HasForeignKey(c => c.ProductId);

            modelBuilder.Entity<Address>().HasMany<Customers>(c=>c.Customers)
                .WithOne(c=>c.Address).HasForeignKey(c => c.AddressId);
        }
    }
}

using GradProject_API.Helpers;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GradProject_API.Models
{
    public class GradDbContext : IdentityDbContext<ApplicationUser>
    {
        public GradDbContext(DbContextOptions<GradDbContext> options) : base(options)
        {
        }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<CartItem> CartItems { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Charity> Charities { get; set; }
        public virtual DbSet<Donation> Donations { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Rating> Ratings { get; set; }
        public DbSet<RentalRequest> RentalRequests { get; set; }
        public DbSet<SwapRequest> SwapRequests { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Seed();

            modelBuilder.Entity<Cart>()
                .HasOne(c => c.User)
                .WithOne()
                .HasForeignKey<Cart>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Cart)
                .WithMany(c => c.CartItems)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Product)
                .WithMany()
                .HasForeignKey(ci => ci.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany()
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Donation>()
                .HasOne(d => d.Charity)
                .WithMany()
                .HasForeignKey(d => d.CharityId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Rating>()
                .HasOne(r => r.Product)
                .WithMany(p => p.Ratings)
                .HasForeignKey(r => r.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Rating>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RentalRequest>()
                .HasOne(r => r.Product)
                .WithMany(r => r.RentalRequests)
                .HasForeignKey(r => r.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RentalRequest>()
                .HasOne(r => r.Renter)
                .WithMany()
                .HasForeignKey(r => r.RenterId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SwapRequest>()
               .HasOne(r => r.Requester)
               .WithMany()
               .HasForeignKey(r => r.RequesterId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SwapRequest>()
                .HasOne(s => s.RequestedProduct)
                .WithMany(s => s.SwapRequests)
                .HasForeignKey(s => s.RequestedProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SwapRequest>()
                .HasOne(s => s.OfferedProduct)
                .WithMany()
                .HasForeignKey(s => s.OfferedProductId)
                .OnDelete(DeleteBehavior.Restrict);


        }
    }
    
}

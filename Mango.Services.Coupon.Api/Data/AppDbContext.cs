using Mango.Services.Coupon.Api.Models;
using Microsoft.EntityFrameworkCore;
namespace Mango.Services.Coupon.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<CouponEntity> Coupons { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CouponEntity>().HasData(
                new CouponEntity()
                {
                    CouponId = 1,
                    CouponCode = "10OFF",
                    DiscountAmount = 10,
                    MinAmount = 20
                });
            modelBuilder.Entity<CouponEntity>().HasData(
                new CouponEntity()
                {
                    CouponId = 2,
                    CouponCode = "20OFF",
                    DiscountAmount = 20,
                    MinAmount = 40
                });
        }
    }
}

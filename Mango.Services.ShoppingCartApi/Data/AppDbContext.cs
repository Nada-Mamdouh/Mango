using Mango.Services.ShoppingCartApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ShoppingCartApi.Data
{
    public class AppDbContext : DbContext
    {
        //1- ctor
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        //2- Dbsets - tables
        public DbSet<CartHeader> CartHeader { get; set; }
        public DbSet<CartDetails> CartDetails { get; set; }

        ////3- Fluent Api 
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //}

    }
}

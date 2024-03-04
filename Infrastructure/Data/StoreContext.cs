using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category{Id=1, Name="Action",DisplayOrder=1},
                new Category{Id=2, Name="Action",DisplayOrder=2},
                new Category{Id=3, Name="Action",DisplayOrder=3}
            );
        }
    }
}
using Cars.Model;
using Microsoft.EntityFrameworkCore;

namespace Cars.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<CarBrand> CarBrands { get; set; }
        public DbSet<CarModel> CarModels { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CarBrand>()
                .HasMany(b => b.Models)
                .WithOne(m => m.CarBrand)
                .HasForeignKey(m => m.CarBrandId)
                .IsRequired();
        }
    }
}

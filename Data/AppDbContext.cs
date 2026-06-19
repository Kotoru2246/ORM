using Microsoft.EntityFrameworkCore;
using BookManagementApp.Models;

namespace BookManagementApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Dish_BIT240128> Dishes { get; set; }
        public DbSet<DishCategory_BIT240128> DishCategories { get; set; }
        public DbSet<DishImage_BIT240128> DishImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dish_BIT240128>().ToTable("Dishes_BIT240128");
            modelBuilder.Entity<DishCategory_BIT240128>().ToTable("DishCategories_BIT240128");
            modelBuilder.Entity<DishImage_BIT240128>().ToTable("DishImages_BIT240128");

            modelBuilder.Entity<Dish_BIT240128>()
                .HasIndex(d => new { d.DishCategoryId, d.Name })
                .IsUnique()
                .HasDatabaseName("UX_Dishes_Category_Name");

            modelBuilder.Entity<DishCategory_BIT240128>()
                .HasMany(c => c.Dishes)
                .WithOne(d => d.DishCategory)
                .HasForeignKey(d => d.DishCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Dish_BIT240128>()
                .HasMany(d => d.DishImages)
                .WithOne(i => i.Dish)
                .HasForeignKey(i => i.DishId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed categories
            modelBuilder.Entity<DishCategory_BIT240128>().HasData(
                new DishCategory_BIT240128 { Id = 1, Name = "Mon khai vi", Description = "Cac mon an khai vi" },
                new DishCategory_BIT240128 { Id = 2, Name = "Mon chinh", Description = "Cac mon an chinh" },
                new DishCategory_BIT240128 { Id = 3, Name = "Trang mieng", Description = "Mon ngot sau bua an" }
            );

            // Seed dishes
            modelBuilder.Entity<Dish_BIT240128>().HasData(
                new Dish_BIT240128 { Id = 1, Name = "Goi cuon", Price = 45000m, PreparationTime = 10, IsAvailable = true, Description = "Goi cuon tuoi mat", DishCategoryId = 1 },
                new Dish_BIT240128 { Id = 2, Name = "Nem ran", Price = 55000m, PreparationTime = 20, IsAvailable = true, Description = "Nem ran gion", DishCategoryId = 1 },
                new Dish_BIT240128 { Id = 3, Name = "Pho bo", Price = 70000m, PreparationTime = 15, IsAvailable = true, Description = "Pho bo truyen thong", DishCategoryId = 2 },
                new Dish_BIT240128 { Id = 4, Name = "Com suon", Price = 80000m, PreparationTime = 25, IsAvailable = true, Description = "Com suon nuong", DishCategoryId = 2 },
                new Dish_BIT240128 { Id = 5, Name = "Kem dua", Price = 40000m, PreparationTime = 5, IsAvailable = true, Description = "Kem dua mat lanh", DishCategoryId = 3 }
            );

            // Seed images
            modelBuilder.Entity<DishImage_BIT240128>().HasData(
                new DishImage_BIT240128 { Id = 1, ImageUrl = "/images/placeholder.svg", IsThumbnail = true, DishId = 1 },
                new DishImage_BIT240128 { Id = 2, ImageUrl = "/images/placeholder.svg", IsThumbnail = true, DishId = 2 },
                new DishImage_BIT240128 { Id = 3, ImageUrl = "/images/placeholder.svg", IsThumbnail = true, DishId = 3 },
                new DishImage_BIT240128 { Id = 4, ImageUrl = "/images/placeholder.svg", IsThumbnail = true, DishId = 4 },
                new DishImage_BIT240128 { Id = 5, ImageUrl = "/images/placeholder.svg", IsThumbnail = true, DishId = 5 }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}

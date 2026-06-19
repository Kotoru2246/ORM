using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BookManagementApp.Data;
using BookManagementApp.Models;

namespace BookManagementApp.Repositories
{
    public class DishQueryParameters
    {
        public string Search { get; set; }
        public int? DishCategoryId { get; set; }
        public bool? IsAvailable { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string SortBy { get; set; } // "price_asc", "price_desc", "prep_asc"
    }

    public class DishRepository
    {
        private readonly AppDbContext _db;
        public DishRepository(AppDbContext db) { _db = db; }

        public IQueryable<Dish_BIT240128> Query(DishQueryParameters p)
        {
            var q = _db.Dishes.Include(d => d.DishImages).AsQueryable();

            if (!string.IsNullOrWhiteSpace(p.Search))
            {
                q = q.Where(d => d.Name.Contains(p.Search));
            }

            if (p.DishCategoryId.HasValue)
            {
                q = q.Where(d => d.DishCategoryId == p.DishCategoryId.Value);
            }

            if (p.IsAvailable.HasValue)
            {
                // When filtering for available items, exclude discontinued by default
                q = q.Where(d => d.IsAvailable == p.IsAvailable.Value);
            }

            if (p.MinPrice.HasValue)
            {
                q = q.Where(d => d.Price >= p.MinPrice.Value);
            }

            if (p.MaxPrice.HasValue)
            {
                q = q.Where(d => d.Price <= p.MaxPrice.Value);
            }

            if (p.MinPrice.HasValue && p.MaxPrice.HasValue && p.MinPrice > p.MaxPrice)
            {
                // invalid range -> return empty queryable
                return Enumerable.Empty<Dish_BIT240128>().AsQueryable();
            }

            switch (p.SortBy)
            {
                case "price_asc": q = q.OrderBy(d => d.Price); break;
                case "price_desc": q = q.OrderByDescending(d => d.Price); break;
                case "prep_asc": q = q.OrderBy(d => d.PreparationTime); break;
                default: q = q.OrderBy(d => d.Id); break;
            }

            return q;
        }

        public async Task<Dish_BIT240128> GetByIdAsync(int id)
        {
            return await _db.Dishes.Include(d => d.DishImages).FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<DishImage_BIT240128> GetImageByIdAsync(int id)
        {
            return await _db.DishImages.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task AddImageAsync(DishImage_BIT240128 image)
        {
            _db.DishImages.Add(image);
            await _db.SaveChangesAsync();
        }

        public async Task SetThumbnailAsync(int imageId)
        {
            var img = await _db.DishImages.FindAsync(imageId);
            if (img == null) return;

            // set all other images for the same dish to false
            var others = _db.DishImages.Where(i => i.DishId == img.DishId && i.Id != imageId && i.IsThumbnail);
            foreach (var o in others)
            {
                o.IsThumbnail = false;
            }

            img.IsThumbnail = true;
            await _db.SaveChangesAsync();
        }

        public async Task DeleteImageAsync(int imageId)
        {
            var img = await _db.DishImages.FindAsync(imageId);
            if (img == null) return;
            _db.DishImages.Remove(img);
            await _db.SaveChangesAsync();
        }

        public async Task AddAsync(Dish_BIT240128 dish)
        {
            _db.Dishes.Add(dish);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Dish_BIT240128 dish)
        {
            _db.Dishes.Update(dish);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Dish_BIT240128 dish)
        {
            _db.Dishes.Remove(dish);
            await _db.SaveChangesAsync();
        }
    }
}
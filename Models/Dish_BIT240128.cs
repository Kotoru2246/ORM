using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace BookManagementApp.Models
{
    public class Dish_BIT240128 : IValidatableObject
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Không được để trống")]
        public string Name { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0")]
        public decimal Price { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Thời gian chế biến phải lớn hơn 0")]
        public int PreparationTime { get; set; }

        public bool IsAvailable { get; set; }

        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng chọn loại món")]
        public int DishCategoryId { get; set; }

        public DishCategory_BIT240128? DishCategory { get; set; }

        public ICollection<DishImage_BIT240128> DishImages { get; set; } = new List<DishImage_BIT240128>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var db = validationContext.GetService<BookManagementApp.Data.AppDbContext>();
            if (db == null)
            {
                yield break;
            }

            // Ensure category exists
            if (!db.DishCategories.Any(c => c.Id == DishCategoryId))
            {
                yield return new ValidationResult("Loại món không tồn tại", new[] { nameof(DishCategoryId) });
            }

            // Unique name per category
            var exists = db.Dishes.Any(d => d.Name == Name && d.DishCategoryId == DishCategoryId && d.Id != Id);
            if (exists)
            {
                yield return new ValidationResult("Tên món đã tồn tại trong cùng loại món", new[] { nameof(Name) });
            }
        }
    }
}
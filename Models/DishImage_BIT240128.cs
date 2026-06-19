using System.ComponentModel.DataAnnotations;

namespace BookManagementApp.Models
{
    public class DishImage_BIT240128
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Đường dẫn ảnh không được để trống")]
        public string ImageUrl { get; set; }

        public bool IsThumbnail { get; set; }

        public int DishId { get; set; }

        public Dish_BIT240128 Dish { get; set; }
    }
}
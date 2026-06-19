using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookManagementApp.Models
{
    public class DishCategory_BIT240128
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên loại không được để trống")]
        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<Dish_BIT240128> Dishes { get; set; }
    }
}
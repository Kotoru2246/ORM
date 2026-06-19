using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BookManagementApp.Data;

namespace BookManagementApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _db;
        public AdminController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetImages()
        {
            var placeholder = "/images/placeholder.svg";
            var images = _db.DishImages.ToList();
            foreach (var img in images)
            {
                img.ImageUrl = placeholder;
            }
            await _db.SaveChangesAsync();
            TempData["AdminMessage"] = "Đã thay đổi tất cả đường dẫn ảnh sang placeholder.";
            return RedirectToAction(nameof(Index));
        }
    }
}
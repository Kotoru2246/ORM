using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BookManagementApp.Data;
using BookManagementApp.Models;
using BookManagementApp.Repositories;

namespace BookManagementApp.Controllers
{
    public class DishController : Controller
    {
        private readonly AppDbContext _db;
        private readonly DishRepository _repo;
        private readonly Microsoft.AspNetCore.Hosting.IWebHostEnvironment _env;

        public DishController(AppDbContext db, Microsoft.AspNetCore.Hosting.IWebHostEnvironment env)
        {
            _db = db;
            _repo = new DishRepository(db);
            _env = env;
        }

        public IActionResult Index(string search, int? categoryId, bool? isAvailable, decimal? minPrice, decimal? maxPrice, string sortBy)
        {
            var vm = new DishQueryParameters
            {
                Search = search,
                DishCategoryId = categoryId,
                IsAvailable = isAvailable,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                SortBy = sortBy
            };

            ViewBag.Categories = new SelectList(_db.DishCategories.OrderBy(c => c.Name).ToList(), "Id", "Name", categoryId);
            ViewBag.Search = search;
            ViewBag.CategoryId = categoryId;
            // If the request did not include isAvailable parameter, default to showing only available items
            bool isAvailableProvided = Request.Query.ContainsKey("isAvailable");
            if (!isAvailableProvided && !isAvailable.HasValue)
            {
                isAvailable = true; // default to available
            }
            ViewBag.IsAvailable = isAvailable;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;
            ViewBag.SortBy = sortBy;

            // isAvailable select list
            var isAvailableItems = new[] {
                new { Value = "", Text = "Tất cả" },
                new { Value = "true", Text = "Đang bán" },
                new { Value = "false", Text = "Ngừng bán" }
            };
            ViewBag.IsAvailableList = new SelectList(isAvailableItems, "Value", "Text", isAvailable.HasValue ? isAvailable.Value.ToString().ToLower() : "true");

            var sortItems = new[] {
                new { Value = "", Text = "Mặc định" },
                new { Value = "price_asc", Text = "Giá tăng dần" },
                new { Value = "price_desc", Text = "Giá giảm dần" },
                new { Value = "prep_asc", Text = "Thời gian tăng dần" }
            };
            ViewBag.SortList = new SelectList(sortItems, "Value", "Text", sortBy ?? "");

            vm.IsAvailable = isAvailable;
            var query = _repo.Query(vm);

            if (minPrice.HasValue && maxPrice.HasValue && minPrice > maxPrice)
            {
                ViewBag.Error = "Khoảng giá không hợp lệ";
                return View(Enumerable.Empty<Dish_BIT240128>());
            }

            var list = query.ToList();

            if (!list.Any())
            {
                ViewBag.Message = "Không tìm thấy kết quả.";
            }

            return View(list);
        }

        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_db.DishCategories.OrderBy(c => c.Name).ToList(), "Id", "Name");
            return View(new Dish_BIT240128 { IsAvailable = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Dish_BIT240128 dish)
        {
            if (!_db.DishCategories.Any(c => c.Id == dish.DishCategoryId))
            {
                ModelState.AddModelError("DishCategoryId", "Loại món không tồn tại");
            }

            if (_db.Dishes.Any(d => d.Name == dish.Name && d.DishCategoryId == dish.DishCategoryId))
            {
                ModelState.AddModelError("Name", "Tên món đã tồn tại trong cùng loại");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(_db.DishCategories.OrderBy(c => c.Name).ToList(), "Id", "Name");
                return View(dish);
            }

            await _repo.AddAsync(dish);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var dish = await _repo.GetByIdAsync(id);
            if (dish == null) return NotFound();
            ViewBag.Categories = new SelectList(_db.DishCategories.OrderBy(c => c.Name).ToList(), "Id", "Name", dish.DishCategoryId);
            return View(dish);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Dish_BIT240128 dish)
        {
            if (id != dish.Id) return BadRequest();

            if (!_db.DishCategories.Any(c => c.Id == dish.DishCategoryId))
            {
                ModelState.AddModelError("DishCategoryId", "Selected category does not exist");
            }

            if (_db.Dishes.Any(d => d.Id != dish.Id && d.Name == dish.Name && d.DishCategoryId == dish.DishCategoryId))
            {
                ModelState.AddModelError("Name", "Dish name must be unique within the same category");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(_db.DishCategories.OrderBy(c => c.Name).ToList(), "Id", "Name", dish.DishCategoryId);
                return View(dish);
            }

            await _repo.UpdateAsync(dish);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var dish = await _repo.GetByIdAsync(id);
            if (dish == null) return NotFound();
            return View(dish);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddImage(int dishId, string imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
            {
                TempData["ImageError"] = "Đường dẫn ảnh không được để trống.";
                return RedirectToAction(nameof(Details), new { id = dishId });
            }

            var dish = await _repo.GetByIdAsync(dishId);
            if (dish == null) return NotFound();

            var img = new DishImage_BIT240128 { ImageUrl = imageUrl.Trim(), IsThumbnail = false, DishId = dishId };
            await _repo.AddImageAsync(img);
            return RedirectToAction(nameof(Details), new { id = dishId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ImportImage(int dishId, string remoteUrl, bool makeThumbnail = false)
        {
            if (string.IsNullOrWhiteSpace(remoteUrl))
            {
                TempData["ImageError"] = "Đường dẫn ảnh không được để trống.";
                return RedirectToAction(nameof(Details), new { id = dishId });
            }

            // validate dish exists
            var dish = await _repo.GetByIdAsync(dishId);
            if (dish == null) return NotFound();

            try
            {
                using var http = new System.Net.Http.HttpClient();
                http.Timeout = System.TimeSpan.FromSeconds(15);
                using var resp = await http.GetAsync(remoteUrl, System.Net.Http.HttpCompletionOption.ResponseHeadersRead);
                if (!resp.IsSuccessStatusCode)
                {
                    TempData["ImageError"] = "Không thể tải ảnh từ URL.";
                    return RedirectToAction(nameof(Details), new { id = dishId });
                }

                var ct = resp.Content.Headers.ContentType?.MediaType ?? string.Empty;
                if (!ct.StartsWith("image/"))
                {
                    TempData["ImageError"] = "URL không phải là ảnh.";
                    return RedirectToAction(nameof(Details), new { id = dishId });
                }

                // size limit 5 MB
                var maxBytes = 5 * 1024 * 1024;
                var length = resp.Content.Headers.ContentLength ?? -1;
                if (length > maxBytes)
                {
                    TempData["ImageError"] = "Kích thước ảnh quá lớn (tối đa 5MB).";
                    return RedirectToAction(nameof(Details), new { id = dishId });
                }

                var ext = ".jpg";
                if (ct == "image/png") ext = ".png";
                else if (ct == "image/gif") ext = ".gif";
                else if (ct == "image/webp") ext = ".webp";

                var imagesFolder = System.IO.Path.Combine(_env.WebRootPath, "images");
                if (!System.IO.Directory.Exists(imagesFolder)) System.IO.Directory.CreateDirectory(imagesFolder);

                var fileName = System.Guid.NewGuid().ToString() + ext;
                var filePath = System.IO.Path.Combine(imagesFolder, fileName);

                using (var stream = await resp.Content.ReadAsStreamAsync())
                using (var fs = System.IO.File.Create(filePath))
                {
                    await stream.CopyToAsync(fs);
                }

                var storedUrl = "/images/" + fileName;
                var img = new DishImage_BIT240128 { ImageUrl = storedUrl, IsThumbnail = false, DishId = dishId };
                await _repo.AddImageAsync(img);

                if (makeThumbnail)
                {
                    // ensure thumbnail set after add (img.Id is set after save)
                    await _repo.SetThumbnailAsync(img.Id);
                }
            }
            catch (System.Exception ex)
            {
                TempData["ImageError"] = "Lỗi khi tải ảnh: " + ex.Message;
            }

            return RedirectToAction(nameof(Details), new { id = dishId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetThumbnail(int imageId)
        {
            var img = await _repo.GetImageByIdAsync(imageId);
            if (img == null) return NotFound();
            await _repo.SetThumbnailAsync(imageId);
            return RedirectToAction(nameof(Details), new { id = img.DishId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteImage(int imageId)
        {
            var img = await _repo.GetImageByIdAsync(imageId);
            if (img == null) return NotFound();
            int dishId = img.DishId;
            await _repo.DeleteImageAsync(imageId);
            return RedirectToAction(nameof(Details), new { id = dishId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var dish = await _repo.GetByIdAsync(id);
            if (dish == null) return NotFound();
            await _repo.DeleteAsync(dish);
            return RedirectToAction(nameof(Index));
        }
    }
}
using BookManagementApp.Data;
using BookManagementApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BookManagementApp.Controllers
{
    public class BookController : Controller
    {
        private readonly BookRepository _bookRepository;
        private readonly AppDbContext _context;

        public BookController(BookRepository bookRepository, AppDbContext context)
        {
            _bookRepository = bookRepository;
            _context = context;
        }

        // GET: Book
        public IActionResult Index(string? searchName, string? sortBy)
        {
            ViewBag.SearchName = searchName;
            ViewBag.CurrentSort = sortBy;

            var books = _bookRepository.GetAll(searchName, sortBy);
            return View(books);
        }

        // GET: Book/Details/5
        public IActionResult Details(int id)
        {
            var book = _bookRepository.GetById(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // GET: Book/Create
        public IActionResult Create()
        {
            ViewData["Authors"] = GetAuthorSelectList();
            return View();
        }

        // POST: Book/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Book book)
        {
            if (ModelState.IsValid)
            {
                _bookRepository.Add(book);
                return RedirectToAction(nameof(Index));
            }

            ViewData["Authors"] = GetAuthorSelectList(book.AuthorId);
            return View(book);
        }

        // GET: Book/Edit/5
        public IActionResult Edit(int id)
        {
            var book = _bookRepository.GetById(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["Authors"] = GetAuthorSelectList(book.AuthorId);
            return View(book);
        }

        // POST: Book/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Book book)
        {
            if (ModelState.IsValid)
            {
                var success = _bookRepository.Update(book);
                if (!success)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["Authors"] = GetAuthorSelectList(book.AuthorId);
            return View(book);
        }

        // GET: Book/Delete/5
        public IActionResult Delete(int id)
        {
            var book = _bookRepository.GetById(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Book/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var success = _bookRepository.Delete(id);
            if (!success)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }

        private IEnumerable<SelectListItem> GetAuthorSelectList(int? selectedAuthorId = null)
        {
            var authors = _context.Authors
                .AsNoTracking()
                .OrderBy(author => author.Name)
                .ToList();

            var items = authors.Select(author => new SelectListItem
            {
                Value = author.Id.ToString(),
                Text = author.Name,
                Selected = selectedAuthorId.HasValue && selectedAuthorId.Value == author.Id
            }).ToList();

            items.Insert(0, new SelectListItem
            {
                Value = "0",
                Text = "-- Chọn tác giả --",
                Selected = !selectedAuthorId.HasValue || selectedAuthorId == 0
            });

            return items;
        }
    }
}

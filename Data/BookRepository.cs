using BookManagementApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BookManagementApp.Data
{
    /// <summary>
    /// Thực hiện các thao tác CRUD với bảng Book qua Entity Framework Core.
    /// </summary>
    public class BookRepository
    {
        private readonly AppDbContext _context;

        public BookRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Book> GetAll(string? searchName = null, string? sortBy = null)
        {
            var query = _context.Books
                .Include(b => b.Author)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchName))
            {
                query = query.Where(b => EF.Functions.Like(b.Name, $"%{searchName.Trim()}%"));
            }

            query = sortBy?.ToLowerInvariant() switch
            {
                "price_asc" => query.OrderBy(b => b.Price).ThenBy(b => b.Name),
                "price_desc" => query.OrderByDescending(b => b.Price).ThenBy(b => b.Name),
                _ => query.OrderBy(b => b.Id)
            };

            return query.ToList(); // SELECT * FROM Books ORDER BY Id
        }

        public Book? GetById(int id)
        {
            return _context.Books
                .Include(b => b.Author)
                .FirstOrDefault(b => b.Id == id); // SELECT * FROM Books WHERE Id = @id
        }

        public void Add(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
        }

        public bool Update(Book book)
        {
            var existing = _context.Books.Find(book.Id);
            if (existing == null)
            {
                return false;
            }

            existing.Name = book.Name;
            existing.Price = book.Price;
            existing.Description = book.Description;
            existing.AuthorId = book.AuthorId;
            existing.ImageUrl = book.ImageUrl;
            _context.SaveChanges(); // lưu thay đổi vào database
            return true;
        }

        public bool Delete(int id)
        {
            var book = _context.Books.Find(id);
            if (book == null)
            {
                return false;
            }

            _context.Books.Remove(book); // DELETE FROM Books WHERE Id = @id
            _context.SaveChanges();
            return true;
        }
    }
}

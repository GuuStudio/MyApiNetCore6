using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using MyApiNetCore6.Data;
using MyApiNetCore6.Models;
using System.Net.WebSockets;

namespace MyApiNetCore6.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly BookStoreContext _context;

        public BookRepository(BookStoreContext context) {
            _context = context;

        }
        public async Task<int> AddBookAsync(BookModel model)
        {
            var newBook = new Book()
            {
                Title = model.Title,
                Description = model.Description,
                Price   = model.Price,
                Quantity = model.Quantity            
            };
            _context.books!.Add(newBook);
            await _context.SaveChangesAsync();
            return newBook.Id;
        }

        public async Task DeleteBookAsync(int id)
        {
            var deleteBook = _context.books!.SingleOrDefault(book => book.Id == id);
            if(deleteBook != null)
            {
                _context.books!.Remove(deleteBook);
                await _context.SaveChangesAsync();
            }
            throw new Exception("Notfound()");
        }

        public async Task<IEnumerable<BookModel>> GetAllBookAsync()
        {
            return  await _context.books!.Select( book => new BookModel ( )
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                Price = book.Price,
                Quantity = book.Quantity
            }).ToListAsync();

        }

        public async Task<BookModel> GetBookByIdAsync(int id)
        {
            var book = await _context.books!.FindAsync(id);
            if (book != null)
            {
                return new BookModel()
                {
                    Id = book.Id,
                    Title = book.Title,
                    Description = book.Description,
                    Price = book.Price,
                    Quantity = book.Quantity
                };
            }
            return null!;
        }

        public async Task UpdateBookAsync(int id, BookModel model)
        {
            if ( id == model.Id )
            {
                var updateBook = new Book() { 
                    Id = id, 
                    Title = model.Title, 
                    Description = model.Description, 
                    Price = model.Price, 
                    Quantity = model.Quantity 
                };
                _context.books!.Update(updateBook);
                await _context.SaveChangesAsync();
            } else
            {
                throw new Exception("Notfound()");
            }
        }
    }
}

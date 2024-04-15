using Microsoft.EntityFrameworkCore.Metadata;
using MyApiNetCore6.Data;
using MyApiNetCore6.Models;

namespace MyApiNetCore6.Repositories
{
    public interface IBookRepository
    {
        public Task<IEnumerable<BookModel>> GetAllBookAsync();
        public Task<BookModel> GetBookByIdAsync(int id);
        public Task<int> AddBookAsync(BookModel model);
        public Task UpdateBookAsync(int id, BookModel model);
        public Task DeleteBookAsync(int id);

    }
}

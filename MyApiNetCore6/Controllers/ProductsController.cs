using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApiNetCore6.Data;
using MyApiNetCore6.Helpers;
using MyApiNetCore6.Models;
using MyApiNetCore6.Repositories;
using System.Runtime.CompilerServices;

namespace MyApiNetCore6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IBookRepository _bookRepo;
        private readonly BookStoreContext _context;

        public ProductsController( IBookRepository bookRepo, BookStoreContext context) { 
            _bookRepo = bookRepo;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetAllBooks()
        {
            try
            {
                return Ok(await _bookRepo.GetAllBookAsync());
            } catch
            {
                return BadRequest();
            }
        }
        [HttpGet("{id}")]
        // api/Products/5
        public async Task<IActionResult> GetBookById(int id)
        {
            var book = await _bookRepo.GetBookByIdAsync(id);
            return book == null ? NotFound() : Ok(book);
        }
        [HttpPost]
        [Authorize(Roles = AppRole.Customer)]
        public async Task<IActionResult> AddNewBook( BookModel bookModel) {
            try
            {
                var newBookId = await _bookRepo.AddBookAsync(bookModel);
                return CreatedAtAction(nameof(GetBookById), new { id = newBookId }, bookModel);
            } catch
            {
                return BadRequest();
            }
        }
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateBook([FromRoute] int id, [FromBody] BookModel model)
        {
            try
            {
                await _bookRepo.UpdateBookAsync(id, model);
                return Ok();
            } catch
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook([FromRoute] int id)
        {
            try
            {
                await _bookRepo.DeleteBookAsync(id);
                return Ok();
            } catch {
                return BadRequest();
            }

        }
    }
}

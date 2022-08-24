using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using library.Models;

namespace library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookListsController : ControllerBase
    {
        private readonly VirtualLibraryContext _context;

        public BookListsController(VirtualLibraryContext context)
        {
            _context = context;
        }

        // GET: api/BookLists
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BooksList>>> GetBooksLists()
        {
          if (_context.BooksLists == null)
          {
              return NotFound();
          }
            return await _context.BooksLists.ToListAsync();
        }

        // GET: api/BookLists/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BooksList>> GetBooksList(int id)
        {
          if (_context.BooksLists == null)
          {
              return NotFound();
          }
            var booksList = await _context.BooksLists.FindAsync(id);

            if (booksList == null)
            {
                return NotFound();
            }

            return booksList;
        }

        // PUT: api/BookLists/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBooksList(int id, BooksList booksList)
        {
            if (id != booksList.BookId)
            {
                return BadRequest();
            }

            _context.Entry(booksList).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BooksListExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/BookLists
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BooksList>> PostBooksList(BooksList booksList)
        {
          if (_context.BooksLists == null)
          {
              return Problem("Entity set 'VirtualLibraryContext.BooksLists'  is null.");
          }
            _context.BooksLists.Add(booksList);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBooksList", new { id = booksList.BookId }, booksList);
        }

        // DELETE: api/BookLists/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooksList(int id)
        {
            if (_context.BooksLists == null)
            {
                return NotFound();
            }
            var booksList = await _context.BooksLists.FindAsync(id);
            if (booksList == null)
            {
                return NotFound();
            }

            _context.BooksLists.Remove(booksList);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BooksListExists(int id)
        {
            return (_context.BooksLists?.Any(e => e.BookId == id)).GetValueOrDefault();
        }
    }
}

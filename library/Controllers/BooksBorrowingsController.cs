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
    public class BooksBorrowingsController : ControllerBase
    {
        private readonly VirtualLibraryContext _context;
      

        public BooksBorrowingsController(VirtualLibraryContext context)
        {
            _context = context;
            
        }

     


        // GET: api/BooksBorrowings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BooksBorrowing>>> GetBooksBorrowings()
        {
          if (_context.BooksBorrowings == null)
          {
              return NotFound();
          }
            return await _context.BooksBorrowings.ToListAsync();
        }

        // GET: api/BooksBorrowings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BooksBorrowing>> GetBooksBorrowing(int id)
        {
          if (_context.BooksBorrowings == null)
          {
              return NotFound();
          }
            var booksBorrowing = await _context.BooksBorrowings.FindAsync(id);

            if (booksBorrowing == null)
            {
                return NotFound();
            }

            return booksBorrowing;
        }

        // PUT: api/BooksBorrowings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBooksBorrowing(int id, BooksBorrowing booksBorrowing)
        {
            if (id != booksBorrowing.Id)
            {
                return BadRequest();
            }

            _context.Entry(booksBorrowing).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BooksBorrowingExists(id))
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

        // POST: api/BooksBorrowings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BooksBorrowing>> PostBooksBorrowing(BooksBorrowing booksBorrowing)
        {
          if (_context.BooksBorrowings == null)
          {
              return Problem("Entity set 'VirtualLibraryContext.BooksBorrowings'  is null.");
          }
            _context.BooksBorrowings.Add(booksBorrowing);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (BooksBorrowingExists(booksBorrowing.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetBooksBorrowing", new { id = booksBorrowing.Id }, booksBorrowing);
        }

        // DELETE: api/BooksBorrowings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooksBorrowing(int id)
        {
            if (_context.BooksBorrowings == null)
            {
                return NotFound();
            }
            var booksBorrowing = await _context.BooksBorrowings.FindAsync(id);
            if (booksBorrowing == null)
            {
                return NotFound();
            }

            _context.BooksBorrowings.Remove(booksBorrowing);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BooksBorrowingExists(int id)
        {
            return (_context.BooksBorrowings?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

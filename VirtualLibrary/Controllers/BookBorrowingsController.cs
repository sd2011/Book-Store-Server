using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VirtualLibrary.Models;
using VirtualLibrary.Helpers;

namespace VirtualLibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookBorrowingsController : ControllerBase
    {
        private readonly VirtualLibraryContext _context;
        private readonly IJwtService _jwtService;
        private readonly ILogger _logger;
        private readonly IBookBorrowingsService _bookBorrowingsService;

        public BookBorrowingsController
        (
            VirtualLibraryContext context,
            IJwtService jwtService,
            IBookBorrowingsService bookBorrowingsService,
            ILogger<BookBorrowingsController> logger
        )
        {
            _context = context;
            _jwtService = jwtService;
            _logger = logger;
            _bookBorrowingsService = bookBorrowingsService;
        }

        // GET: api/BookBorrowings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BooksBorrowing>>> GetBooksBorrowings()
        {
          if (_context.BooksBorrowings == null)
          {
              return NotFound();
          }
            return await _context.BooksBorrowings.ToListAsync();
        }

        // GET: api/BookBorrowings/5
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

        // PUT: api/BookBorrowings/5
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

        // POST: api/BookBorrowings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BooksBorrowing>> PostBooksBorrowing(int bookId)
        {
          if (_context.BooksBorrowings == null || _context.Customers == null || _context.BooksLists == null)
          {
              return Problem("Entity set 'VirtualLibraryContext.BooksBorrowings'  is null.");
          }
          try
           {
             _logger.LogInformation("----------------------------------------reqC: {BB} ", Request.Cookies);
            var jwt = Request.Cookies["jwt"];
            _logger.LogInformation("----------------------------------------jwt: {BB} ", jwt);

            
            if(jwt == null){
                return Problem("Customer not loged in");
            }

            var token = _jwtService.Verify(jwt);
        
            int userId = int.Parse(token.Issuer);

           // var allCustomers = await _context.Customers.ToListAsync();
             
             var theCustomer = _context.Customers.Find(userId);

             var theBook = _context.BooksLists.Find(bookId);

              _logger.LogInformation("----------------------------------------book: {BB} ", theBook);
           
             _logger.LogInformation("----------------------------------------theCustomer: {BB} ", theCustomer);

             if(theCustomer == null || theBook == null)
             {
              return Problem("User or book dose not exists on DB."); 
             }
             
             
             if( await _bookBorrowingsService.IsUserHaveBook(userId,bookId)) 
             {
                return Problem("User Allready lented Book"); 
             }

             if(theBook.NumOfCopies < 1)
             {
              return Problem("No copies of the book left."); 
                    
             }



             BooksBorrowing bookedBorrowed = new BooksBorrowing(userId, bookId);
            _logger.LogInformation("----------------------------------------bookedBorrowed: {BB} ", bookedBorrowed);


              _context.BooksBorrowings.Add(bookedBorrowed);

              theBook.NumOfCopies--;

              _context.Entry(theBook).State = EntityState.Modified;

             await _context.SaveChangesAsync();

            return CreatedAtAction("GetBooksBorrowing", new { id = userId }, bookedBorrowed);

           }
            catch (DbUpdateException)
            {
                if (BooksBorrowingExists(bookId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception)
            {
                return Unauthorized();
            }

        }

        // DELETE: api/BookBorrowings/5
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
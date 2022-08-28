using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VirtualLibrary.Helpers;
using VirtualLibrary.Models;

namespace VirtualLibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly VirtualLibraryContext _context;

    private readonly IJwtService _jwtService;
    private readonly IReviewService _reviewService;
    private readonly IBookBorrowingsService _bookBorrowingsService;
        public ReviewsController(
            VirtualLibraryContext context, 
            IJwtService jwtService, 
            IReviewService reviewService,
            IBookBorrowingsService bookBorrowingsService
            )
        {
            _context = context;
            _jwtService = jwtService;
            _bookBorrowingsService =  bookBorrowingsService;
            _reviewService = reviewService;
        }

        // GET: api/Reviews
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviews()
        {
          if (_context.Reviews == null)
          {
              return NotFound();
          }
            return await _context.Reviews.ToListAsync();
        }

        // GET: api/Reviews/withUserName/5
        [HttpGet("withUserName/{id}")]
        //notice id is bookid
        public async Task<ActionResult<IEnumerable<ReviewAndCustomerName>>> GetReview(int id)
        {
          if (_context.Reviews == null)
          {
              return NotFound();
          }
            var reviewsAndCustomerNames = await _reviewService.getBookReviewsWithUserName(id);

            if (reviewsAndCustomerNames == null)
            {
                return NotFound();
            }

            return reviewsAndCustomerNames.ToList();
        }

        // PUT: api/Reviews/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReview(int id, Review review)
        {
            if (id != review.Id)
            {
                return BadRequest();
            }

            _context.Entry(review).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewExists(id))
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

         [HttpGet("getAvgRate/{id}")]
        //notice id is bookid
        public async Task<ActionResult<double>> getAvgRate(int id)
        {
          if (_context.Reviews == null)
          {
              return NotFound();
          }
        return  await _reviewService.getAvgRate(id);

        }

        // POST: api/Reviews
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Review>> PostReview(string title, string reviewText ,int rate, int bookId)
        {
          if (_context.Reviews == null)
          {
              return Problem("Entity set 'VirtualLibraryContext.Reviews'  is null.");
          }

        try{
          var jwt = Request.Cookies["jwt"];

          if(jwt == null){
                return Problem("Customer not loged in");
            }

          var token = _jwtService.Verify(jwt);

          var userId = int.Parse(token.Issuer);

          var theCustomer = _context.Customers.Find(userId);

             var theBook = _context.BooksLists.Find(bookId);

           

             if(theCustomer == null || theBook == null)
             {
              return Problem("User or book dose not exists on DB."); 
             }
             
             
             if( await _reviewService.IsUserHaveReview(userId,bookId)) 
             {
                return Problem("User allready reviewd book"); 
             }

        

             Review theReview = new Review(userId, title, reviewText , rate, bookId);


              _context.Reviews.Add(theReview);

             await _context.SaveChangesAsync();

            return CreatedAtAction("GetReviews", new { id = userId }, theReview);
        }
            catch (DbUpdateException)
            {
                throw;
            }

        }

        // DELETE: api/Reviews/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            if (_context.Reviews == null)
            {
                return NotFound();
            }
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReviewExists(int id)
        {
            return (_context.Reviews?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

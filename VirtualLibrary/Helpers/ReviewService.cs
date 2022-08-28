using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtualLibrary.Controllers;
using VirtualLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VirtualLibrary.Models;

namespace VirtualLibrary.Helpers
{

    public interface IReviewService
    {   
        public Task<bool> IsUserHaveReview(int id, int bookId);
    public Task<IEnumerable<ReviewAndCustomerName>> getBookReviewsWithUserName(int bookId);

    public Task<double> getAvgRate(int bookId);

        
        
    }
    public class ReviewService : IReviewService
    {

        private readonly VirtualLibraryContext _context;
        ILogger<ReviewsController> _logger;

    public ReviewService(VirtualLibraryContext context,ILogger<ReviewsController> logger)
    {
        _context = context;
        _logger = logger;

    }
        public async Task<bool> IsUserHaveReview(int userId, int bookId)
        {
          

        var table = await _context.Reviews.Where(r => r.BookId == bookId && r.Id == userId ).ToListAsync();
 
            return table.Count >0;
        }

        public async Task<IEnumerable<ReviewAndCustomerName>> getBookReviewsWithUserName(int bookId){
            

        
        return 
         from r in  _context.Reviews
        where r.BookId == bookId
        join c in _context.Customers
        on r.Id equals c.Id
            select new ReviewAndCustomerName(
                r.Id,
                r.ReviewDate,
                r.Title,
                r.ReviewText,
                r.Rate,
                r.BookId,
                c.Email   
            );
    }
    public async Task<double> getAvgRate(int bookId){

        var rates = from review in _context.Reviews
                where review.BookId == bookId
                select review.Rate;

            return await rates.AverageAsync();
     }
    }
}
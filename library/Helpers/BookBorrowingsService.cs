using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using library.Controllers;
using library.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace library.Helpers
{

    public interface IBookBorrowingsService
    {   
        public Task<bool> IsUserHaveBook(int id, int bookId);
    }
    public class BookBorrowingsService : IBookBorrowingsService
    {

        private readonly VirtualLibraryContext _context;
        ILogger<BookBorrowingsController> _logger;

    public BookBorrowingsService(VirtualLibraryContext context,ILogger<BookBorrowingsController> logger)
    {
        _context = context;
        _logger = logger;

    }
        public async Task<bool> IsUserHaveBook(int userId, int bookId)
        {
            string procedure = @"
	    @id = {0} ,
		@bookId ={2}  
        AS 
	    BEGIN
            SELECT bookId
            FROM BooksBorrowing 
            WHERE bookId = @bookId and id = @id 
        END";

        var table = await _context.BooksBorrowings.Where(bb => bb.BookId == bookId && bb.Id == userId ).ToListAsync();

    _logger.LogInformation("----------------------------------------/n table: {T} ", table);

            return (table.Count != 0);
        }
    }
}
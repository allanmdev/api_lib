using api_lib.Data;
using api_lib.Models;
using api_lib.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace api_lib.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RentsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RentBook>>> GetRents()
        {
            return await _context.RentedBooks
                .Include(r => r.Book)
                .Include(r => r.User)
                .OrderBy(r => r.ReturnDate) 
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<RentBook>> CreateRent(RentRequest request)
        {
            var book = await _context.Books.FindAsync(request.BookId);

            if (book == null)
            {
                return BadRequest("Livro não existe.");
            }

            if (book.Quantity <= 0)
            {
                return BadRequest("Não há exemplares disponíveis no momento.");
            }

            var existingRent = await _context.RentedBooks
                .FirstOrDefaultAsync(r => r.BookId == request.BookId && r.UserId == request.UserId);

            if (existingRent != null)
            {
                return BadRequest("Esse livro já foi alugado anteriormente por este usuário.");
            }

            book.Quantity -= 1;

            var rentBook = new RentBook
            {
                BookId = request.BookId,
                UserId = request.UserId,
                CreatedAt = DateTime.UtcNow
            };

            _context.RentedBooks.Add(rentBook);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRents), new { id = rentBook.Id }, rentBook);
        }

        [HttpPost("{bookId}")]
        public async Task<ActionResult> ReturnBook(int bookId, [FromBody] ReturnRequest request)
        {
            var rentBook = await _context.RentedBooks
                .FirstOrDefaultAsync(r => r.BookId == bookId && r.UserId == request.UserId && r.ReturnDate == null);

            if (rentBook == null)
            {
                return NotFound("Nenhuma locação encontrada para o ID fornecido ou o usuário não está autorizado.");
            }

            var book = await _context.Books.FindAsync(rentBook.BookId);
            if (book == null)
            {
                return BadRequest("Livro associado à locação não existe.");
            }

            if (rentBook.ReturnDate != null)
            {
                return BadRequest("Este livro já foi devolvido.");
            }

            book.Quantity += 1;

            rentBook.ReturnDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new { Message = "Livro devolvido com sucesso.", ReturnDate = rentBook.ReturnDate });
        }

        public class ReturnRequest
        {
            public int UserId { get; set; }
        }
    }
}
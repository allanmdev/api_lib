using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using api_lib.Data;
using api_lib.Models;
using api_lib.Requests;
using Microsoft.EntityFrameworkCore;

namespace api_lib.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BooksController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<BookResponse>>>> GetBooks()
        {
            var books = await _context.Books
                .Select(b => new BookResponse
                {
                    Id = b.Id,
                    Title = b.Title,
                    Author = b.Author,
                    Year = b.Year,
                    Quantity = b.Quantity
                }).ToListAsync();

            return Ok(new ApiResponse<IEnumerable<BookResponse>>
            {
                Success = true,
                Message = "Livros obtidos com sucesso.",
                Data = books
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<BookResponse>>> GetBook(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound(new ApiResponse<BookResponse>
                {
                    Success = false,
                    Message = "Livro não encontrado.",
                    Data = null
                });
            }

            var response = new BookResponse
            {
                Title = book.Title,
                Author = book.Author,
                Year = book.Year,
                Quantity = book.Quantity
            };

            return Ok(new ApiResponse<BookResponse>
            {
                Success = true,
                Message = "Livro obtido com sucesso.",
                Data = response
            });
        }

        [HttpPost]
        [Authorize] 
        public async Task<ActionResult<ApiResponse<BookResponse>>> PostBook(BookRequest request)
        {
            if (!IsAdmin())
            {
                return Forbid("Você não tem permissão para criar livros.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<BookResponse>
                {
                    Success = false,
                    Message = "Dados inválidos.",
                    Data = null
                });
            }

            var book = new Book
            {
                Title = request.Title,
                Author = request.Author,
                Year = request.Year,
                Quantity = request.Quantity,
                CreatedAt = DateTime.UtcNow
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            var response = new BookResponse
            {
                Title = book.Title,
                Author = book.Author,
                Year = book.Year,
                Quantity = book.Quantity
            };

            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, new ApiResponse<BookResponse>
            {
                Success = true,
                Message = "Livro criado com sucesso.",
                Data = response
            });
        }

        [HttpDelete("{id}")]
        [Authorize] 
        public async Task<IActionResult> DeleteBook(int id)
        {
            if (!IsAdmin())
            {
                return Forbid("Você não tem permissão para deletar livros.");
            }

            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound(new ApiResponse<BookResponse>
                {
                    Success = false,
                    Message = "Livro não encontrado.",
                    Data = null
                });
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<BookResponse>
            {
                Success = true,
                Message = "Livro deletado com sucesso.",
                Data = null
            });
        }

        [HttpPut("{id}")]
        [Authorize] 
        public async Task<ActionResult<ApiResponse<BookResponse>>> EditBook(int id, BookRequest request)
        {
            if (!IsAdmin())
            {
                return Forbid("Você não tem permissão para editar livros.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<BookResponse>
                {
                    Success = false,
                    Message = "Dados inválidos.",
                    Data = null
                });
            }

            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound(new ApiResponse<BookResponse>
                {
                    Success = false,
                    Message = "Livro não encontrado.",
                    Data = null
                });
            }

            book.Title = request.Title;
            book.Author = request.Author;
            book.Year = request.Year;
            book.Quantity = request.Quantity;

            _context.Books.Update(book);
            await _context.SaveChangesAsync();

            var response = new BookResponse
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Year = book.Year,
                Quantity = book.Quantity
            };

            return Ok(new ApiResponse<BookResponse>
            {
                Success = true,
                Message = "Livro atualizado com sucesso.",
                Data = response
            });
        }
        private bool IsAdmin()
        {
            var isAdminClaim = User.FindFirst("isAdmin")?.Value;
            return isAdminClaim != null && bool.TryParse(isAdminClaim, out var isAdmin) && isAdmin;
        }
    }

    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }

    public class BookResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }
        public int Quantity { get; set; }
    }
}

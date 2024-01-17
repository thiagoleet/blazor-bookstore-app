using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStoreApp.API.Data;
using AutoMapper;
using BookStoreApp.API.Models.Book;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;

namespace BookStoreApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BooksController : ControllerBase
    {
        private readonly BookstoreContext context;
        private readonly IMapper mapper;
        private readonly ILogger<BooksController> logger;

        public BooksController(BookstoreContext context, IMapper mapper, ILogger<BooksController> logger)
        {
            this.context = context;
            this.logger = logger;
            this.mapper = mapper;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookReadOnlyDto>>> GetBooks()
        {
            try
            {
                if (context.Books == null)
                {
                    return NotFound();
                }

                var bookDtos = await context.Books
                 .Include(q => q.Author)
                 .ProjectTo<BookReadOnlyDto>(mapper.ConfigurationProvider)
                 .ToListAsync();

                return Ok(bookDtos);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in BooksController.GetBooks()");
                return Problem(ex.Message);
            }
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDetailsDto>> GetBook(int id)
        {
            try
            {
                if (context.Books == null)
                {
                    return NotFound();
                }

                var book = await context.Books
                    .Include(q => q.Author)
                    .ProjectTo<BookDetailsDto>(mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(q => q.Id == id);

                if (book == null)
                {
                    return NotFound();
                }

                return Ok(book);


            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in BooksController.GetBook()");
                return Problem(ex.Message);
            }
        }

        // PUT: api/Books/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]

        public async Task<IActionResult> PutBook(int id, BookUpdateDto bookDto)
        {
            if (id != bookDto.Id)
            {
                return BadRequest();
            }


            var book = await context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            mapper.Map(bookDto, book);
            context.Entry(book).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await BookExists(id))
                {
                    logger.LogError("Book not found in BooksController.PutBook()");
                    return NotFound();
                }
                else
                {
                    logger.LogError("Error in BooksController.PutBook()");
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Books
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Administrator")]

        public async Task<ActionResult<Book>> PostBook(BookCreateDto bookDto)
        {
            try
            {
                if (context.Books == null)
                {
                    return Problem("Entity set 'BookstoreContext.Books'  is null.");
                }

                var book = mapper.Map<Book>(bookDto);

                context.Books.Add(book);
                await context.SaveChangesAsync();

                return CreatedAtAction("GetBook", new { id = book.Id }, book);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in BooksController.PostBook()");
                return Problem(ex.Message);
            }
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]

        public async Task<IActionResult> DeleteBook(int id)
        {
            try
            {
                if (context.Books == null)
                {
                    return NotFound();
                }
                var book = await context.Books.FindAsync(id);
                if (book == null)
                {
                    return NotFound();
                }

                context.Books.Remove(book);
                await context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in BooksController.DeleteBook()");
                return Problem(ex.Message);
            }
        }

        private async Task<bool> BookExists(int id)
        {
            return await context.Books.AnyAsync(e => e.Id == id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStoreApp.API.Data;
using BookStoreApp.API.Models.Author;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace BookStoreApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class AuthorsController : ControllerBase
    {
        private readonly BookstoreContext context;
        private readonly IMapper mapper;
        private readonly ILogger<AuthorsController> logger;

        public AuthorsController(BookstoreContext context, IMapper mapper, ILogger<AuthorsController> logger)
        {
            this.context = context;
            this.mapper = mapper;
            this.logger = logger;
        }

        // GET: api/Authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorReadOnlyDto>>> GetAuthors()
        {

            try
            {
                if (context.Authors == null)
                {
                    return NotFound();
                }

                var authors = await context.Authors.ToListAsync();
                var authorsDto = mapper.Map<IEnumerable<AuthorReadOnlyDto>>(authors);

                return Ok(authorsDto);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in AuthorsController.GetAuthors()");
                return Problem(ex.Message);
            }
        }

        // GET: api/Authors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorReadOnlyDto>> GetAuthor(int id)
        {
            try
            {
                if (context.Authors == null)
                {
                    return NotFound();
                }
                var author = await context.Authors.FindAsync(id);

                if (author == null)
                {
                    return NotFound();
                }

                var authorDto = mapper.Map<AuthorReadOnlyDto>(author);

                return Ok(authorDto);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in AuthorsController.GetAuthor()");
                return Problem(ex.Message);
            }
        }

        // PUT: api/Authors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> PutAuthor(int id, AuthorUpdateDto authorDto)
        {
            if (id != authorDto.Id)
            {
                return BadRequest();
            }

            var author = await context.Authors.FindAsync(id);

            if (author == null)
            {
                return NotFound();
            }

            mapper.Map(authorDto, author);
            context.Entry(author).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

                if (!await AuthorExists(id))
                {
                    logger.LogError("Author not found in AuthorsController.PutAuthor()");
                    return NotFound();
                }
                else
                {
                    logger.LogError("Error in AuthorsController.PutAuthor()");
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Authors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<Author>> PostAuthor(AuthorCreateDto authorDto)
        {

            try
            {
                if (context.Authors == null)
                {
                    return Problem("Entity set 'BookstoreContext.Authors'  is null.");
                }

                var author = mapper.Map<Author>(authorDto);
                context.Authors.Add(author);
                await context.SaveChangesAsync();

                return CreatedAtAction("GetAuthor", new { id = author.Id }, author);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in AuthorsController.PostAuthor()");
                return Problem(ex.Message);
            }
        }

        // DELETE: api/Authors/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]

        public async Task<IActionResult> DeleteAuthor(int id)
        {
            try
            {
                if (context.Authors == null)
                {
                    return NotFound();
                }
                var author = await context.Authors.FindAsync(id);
                if (author == null)
                {
                    return NotFound();
                }

                context.Authors.Remove(author);
                await context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in AuthorsController.DeleteAuthor()");
                return Problem(ex.Message);
            }
        }

        private async Task<bool> AuthorExists(int id)
        {
            return await context.Authors.AnyAsync(e => e.Id == id);
        }
    }
}

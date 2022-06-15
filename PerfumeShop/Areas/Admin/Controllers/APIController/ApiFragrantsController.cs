using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PerfumeShop.Models;

namespace PerfumeShop.Areas.Admin.Controllers.APIController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiFragrantsController : ControllerBase
    {
        private readonly DBContext _context;

        public ApiFragrantsController(DBContext context)
        {
            _context = context;
        }

        // GET: api/ApiFragrants
        [HttpGet]
        [Route("get-Fragrants")]
        public async Task<ActionResult<IEnumerable<Fragrant>>> GetFragrant()
        {
          if (_context.Fragrant == null)
          {
              return NotFound();
          }
            return await _context.Fragrant.ToListAsync();
        }

        // GET: api/ApiFragrants/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Fragrant>> GetFragrant(int id)
        {
          if (_context.Fragrant == null)
          {
              return NotFound();
          }
            var fragrant = await _context.Fragrant.FindAsync(id);

            if (fragrant == null)
            {
                return NotFound();
            }

            return fragrant;
        }

        // PUT: api/ApiFragrants/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFragrant(int id, Fragrant fragrant)
        {
            if (id != fragrant.FragrantId)
            {
                return BadRequest();
            }

            _context.Entry(fragrant).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FragrantExists(id))
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

        // POST: api/ApiFragrants
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Route("add-fragrant")]
        public async Task<ActionResult<Fragrant>> PostFragrant(Fragrant fragrant)
        {
          if (_context.Fragrant == null)
          {
              return Problem("Entity set 'DBContext.Fragrant'  is null.");
          }
            _context.Fragrant.Add(fragrant);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFragrant", new { id = fragrant.FragrantId }, fragrant);
        }

        // DELETE: api/ApiFragrants/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFragrant(int id)
        {
            if (_context.Fragrant == null)
            {
                return NotFound();
            }
            var fragrant = await _context.Fragrant.FindAsync(id);
            if (fragrant == null)
            {
                return NotFound();
            }

            _context.Fragrant.Remove(fragrant);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FragrantExists(int id)
        {
            return (_context.Fragrant?.Any(e => e.FragrantId == id)).GetValueOrDefault();
        }
    }
}

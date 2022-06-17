using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PerfumeShop.Models;

namespace PerfumeShop.Areas.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiShippersController : ControllerBase
    {
        private readonly DBContext _context;

        public ApiShippersController(DBContext context)
        {
            _context = context;
        }

        // GET: api/ApiShippers
        [HttpGet]
        [Route("get-list-shipper")]
        public async Task<ActionResult<IEnumerable<Shippers>>> GetShippers()
        {
            if (_context.Shippers == null)
            {
                return NotFound();
            }
            return await _context.Shippers.ToListAsync();
        }

        [HttpGet]
        public IActionResult Search(string name)
        {
            var result = _context.Shippers.Where(c => c.Name.StartsWith(name) || c.Name == null).ToList();
            return Ok(result);
        }
        // GET: api/ApiShippers/5
        [HttpGet("{name}")]
        public async Task<ActionResult<Shippers>> GetShippers(string name)
        {
          if (_context.Shippers == null)
          {
              return NotFound();
          }

          var result = await _context.Shippers.Where(c => c.Name.Contains(name)).ToListAsync();
            var shippers = await _context.Shippers.FindAsync(result);

            if (shippers == null)
            {
                return NotFound();
            }

            return shippers;
        }

        // PUT: api/ApiShippers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutShippers(int id, Shippers shippers)
        {
            if (id != shippers.ShipperId)
            {
                return BadRequest();
            }

            _context.Entry(shippers).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShippersExists(id))
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

        // POST: api/ApiShippers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Route("add-shippers")]
        public async Task<ActionResult<Shippers>> PostShippers(Shippers shippers)
        {
          if (_context.Shippers == null)
          {
              return Problem("Entity set 'DBContext.Shippers'  is null.");
          }
            _context.Shippers.Add(shippers);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetShippers", new { id = shippers.ShipperId }, shippers);
        }

        // DELETE: api/ApiShippers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShippers(int id)
        {
            if (_context.Shippers == null)
            {
                return NotFound();
            }
            var shippers = await _context.Shippers.FindAsync(id);
            if (shippers == null)
            {
                return NotFound();
            }

            _context.Shippers.Remove(shippers);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ShippersExists(int id)
        {
            return (_context.Shippers?.Any(e => e.ShipperId == id)).GetValueOrDefault();
        }
    }
}

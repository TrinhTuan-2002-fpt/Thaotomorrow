using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PerfumeShop.Models;

namespace PerfumeShop.Areas.Admin.Controllers.APIController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiProductsController : ControllerBase
    {
        private readonly DBContext _context;
        public ApiProductsController(DBContext context)
        {
            _context = context;
        }
        // GET: api/Products
        [HttpGet]
        [Route("Get-All")]
        public async Task<ActionResult<IEnumerable<Products>>> GetProducts()
        {
          if (_context.Products == null)
          {
              return NotFound();
          }
            return await _context.Products.ToListAsync();
        }
        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Products>> GetProducts(int id)
        {
          if (_context.Products == null)
          {
              return NotFound();
          }
            var products = await _context.Products.FindAsync(id);

            if (products == null)
            {
                return NotFound();
            }

            return products;
        }
        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProducts(int id, Products products)
        {
            if (id != products.ProdcutId)
            {
                return BadRequest();
            }

            _context.Entry(products).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductsExists(id))
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
        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Route("addSP")]
        public async Task<ActionResult<Products>> PostProducts(Products products)
        {
          if (_context.Products == null)
          {
              return Problem("Entity set 'DBContext.Products'  is null.");
          }
            _context.Products.Add(products);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProducts", new { id = products.ProdcutId }, products);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducts(int id)
        {
            if (_context.Products == null)
            {
                return NotFound();
            }
            var products = await _context.Products.FindAsync(id);
            if (products == null)
            {
                return NotFound();
            }

            _context.Products.Remove(products);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductsExists(int id)
        {
            return (_context.Products?.Any(e => e.ProdcutId == id)).GetValueOrDefault();
        }
    }
}

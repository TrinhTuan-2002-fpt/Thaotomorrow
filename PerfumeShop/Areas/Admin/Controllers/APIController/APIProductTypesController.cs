using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PerfumeShop.Models;

namespace PerfumeShop.Areas.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIProductTypesController : ControllerBase
    {
        private readonly DBContext _context;

        public APIProductTypesController(DBContext context)
        {
            _context = context;
        }

        // GET: api/ProductTypes
        [HttpGet]
        [Route("getSP")]
        public async Task<ActionResult<IEnumerable<ProductTypes>>> GetProductTypes()
        {
            if (_context.ProductTypes == null)
            {
                return NotFound();
            }
            return await _context.ProductTypes.ToListAsync();
        }

        // GET: api/ProductTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductTypes>> GetProductTypes(int id)
        {
            if (_context.ProductTypes == null)
            {
                return NotFound();
            }
            var productTypes = await _context.ProductTypes.FindAsync(id);

            if (productTypes == null)
            {
                return NotFound();
            }

            return productTypes;
        }

        // PUT: api/ProductTypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductTypes(int id, ProductTypes productTypes)
        {
            if (id != productTypes.TypeId)
            {
                return BadRequest();
            }

            _context.Entry(productTypes).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductTypesExists(id))
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

        // POST: api/ProductTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Route("addSP")]
        public async Task<string> PostProductTypes(ProductTypes productTypes)
        {
            ProductTypes types = new ProductTypes
            {
                Name = productTypes.Name
            };
            _context.ProductTypes.Add(types);
            await _context.SaveChangesAsync();
            return "ok";
        }

        // DELETE: api/ProductTypes/5
        [HttpDelete("{id}")]
        public async Task<string> DeleteProductTypes(int id)
        {
            var productTypes = await _context.ProductTypes.FindAsync(id);
            

            _context.ProductTypes.Remove(productTypes);
            await _context.SaveChangesAsync();

            return "ok";
        }

        private bool ProductTypesExists(int id)
        {
            return (_context.ProductTypes?.Any(e => e.TypeId == id)).GetValueOrDefault();
        }
    }
}

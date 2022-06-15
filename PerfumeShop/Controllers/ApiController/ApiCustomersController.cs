using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PerfumeShop.Models;

namespace PerfumeShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiCustomersController : ControllerBase
    {
        private readonly DBContext _context;
        
        public ApiCustomersController(DBContext context)
        {
            _context = context;
            
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customers>>> GetCustomers()
        {
            if (_context.Customers == null)
            {
                return NotFound();
            }
            return await _context.Customers.ToListAsync();
        }
        [HttpPost]
        public async Task<ActionResult<Customers>> PostCustomers(Customers customers)
        {
            if (_context.Customers == null)
            {
                return Problem("Entity set 'DBContext.Customers'  is null.");
            }
            _context.Customers.Add(customers);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomers", new { id = customers.CustomerId }, customers);
        }
    }
}

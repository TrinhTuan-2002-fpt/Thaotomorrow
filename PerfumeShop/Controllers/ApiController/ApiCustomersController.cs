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

        ////[HttpGet]
        ////public Task<IActionResult> Login(string email, string password)
        ////{
            
        ////}
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutCustomers(int id, Customers customers)
        //{
        //    if (id != customers.CustomerId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(customers).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!CustomersExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //[HttpPost]
        //public async Task<ActionResult<Customers>> PostCustomers(Customers customers)
        //{
        //  if (_context.Customers == null)
        //  {
        //      return Problem("Entity set 'DBContext.Customers'  is null.");
        //  }
        //  _context.Customers.Add(customers);
        //  await _context.SaveChangesAsync();

        //  return CreatedAtAction("GetCustomers", new { id = customers.CustomerId }, customers);
        //}


        //private bool CustomersExists(int id)
        //{
        //    return (_context.Customers?.Any(e => e.CustomerId == id)).GetValueOrDefault();
        //}
    }
}

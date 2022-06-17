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
        [Route("login")]
        public async Task<ActionResult<Customers>> Login(string email, string password)
        {
            if (_context.Customers == null)
            {
                return NotFound();
            }

            if (String.IsNullOrEmpty(email) && String.IsNullOrEmpty(password))
            {
                return Problem("Email hoặc mật khẩu bị rỗng");
            }

            try
            {
                var info = await _context.Customers.FirstOrDefaultAsync(c =>
                    c.Email == email && c.Password == password);
                if (info == null)
                {
                    return NotFound("Không tìm thấy");
                }

                return info;
            }
            catch (Exception e)
            {
                return Problem("erorr");
            }
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

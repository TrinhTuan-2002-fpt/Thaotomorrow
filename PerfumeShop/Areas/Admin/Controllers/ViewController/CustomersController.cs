using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PerfumeShop.Models;

namespace PerfumeShop.Areas.Admin.Controllers.ViewController
{
    [Area("Admin")]
    public class CustomersController : Controller
    {
        private readonly DBContext _context;
        private readonly HttpClient _client = new HttpClient();

        public CustomersController(DBContext context)
        {
            _context = context;
            _client.BaseAddress = new Uri("https://localhost:7015/");
        }

        // GET: Admin/Customers
        public async Task<IActionResult> Index()
        {
            ViewData["Email"] = HttpContext.Session.GetString("Email");

            var jsonContent = await _client.GetAsync("api/APICustomers/get-Cus");
            var jsonData = await jsonContent.Content.ReadAsStringAsync();

            var model = JsonConvert.DeserializeObject<List<Customers>>(jsonData);

            return View(model);
        }

        // GET: Admin/Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewData["Email"] = HttpContext.Session.GetString("Email");

            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customers = await _context.Customers
                .Include(c => c.Address)
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customers == null)
            {
                return NotFound();
            }

            return View(customers);
        }

        // GET: Admin/Customers/Create
        public async Task<IActionResult> Create()
        {
            ViewData["Email"] = HttpContext.Session.GetString("Email");
            ViewData["AddressId"] = new SelectList(_context.Address, "AddressId", "City");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Customers customers)
        {
            var acc = await _context.Customers.Where(c => c.Email == customers.Email || c.PhoneNumber == customers.PhoneNumber).FirstOrDefaultAsync();
            if (acc != null)
            {
                ViewData["Email"] = acc.Email + " Đã có trong hệ thống";
                ViewData["Phone"] = acc.PhoneNumber + " Đã có trong hệ thống";
                return View();
            }
            if (ModelState.IsValid)
            {
                var myContent = JsonConvert.SerializeObject(customers);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byContent = new ByteArrayContent(buffer);
                byContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                await _client.PostAsync("api/APICustomers/add-customer", byContent);
                return RedirectToAction(nameof(Index));
            }
            ViewData["AddressId"] = new SelectList(_context.Address, "AddressId", "City", customers.AddressId);
            return View(customers);
        }

        // GET: Admin/Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewData["Email"] = HttpContext.Session.GetString("Email");

            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }
            
            var customers = await _context.Customers.FindAsync(id);
            if (customers == null)
            {
                return NotFound();
            }
            ViewData["AddressId"] = new SelectList(_context.Address, "AddressId", "City");
            return View(customers);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Customers customers)
        {
            if (id != customers.CustomerId)
            {
                return NotFound();
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    await _client.PutAsJsonAsync<Customers>($"api/APICustomers/{id}", customers);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomersExists(customers.CustomerId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AddressId"] = new SelectList(_context.Address, "AddressId", "City", customers.AddressId);
            return View(customers);
        }

        // GET: Admin/Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ViewData["Email"] = HttpContext.Session.GetString("Email");
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customers = await _context.Customers
                .Include(c => c.Address)
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customers == null)
            {
                return NotFound();
            }

            return View(customers);
        }

        // POST: Admin/Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Customers == null)
            {
                return Problem("Entity set 'DBContext.Customers'  is null.");
            }

            await _client.DeleteAsync($"api/APICustomers/{id}");
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomersExists(int id)
        {
          return (_context.Customers?.Any(e => e.CustomerId == id)).GetValueOrDefault();
        }
    }
}

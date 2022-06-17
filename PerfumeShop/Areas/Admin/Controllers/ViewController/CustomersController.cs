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
            var jsonContent = await _client.GetAsync("api/APICustomers/get-Cus");
            var jsonData = await jsonContent.Content.ReadAsStringAsync();

            var model = JsonConvert.DeserializeObject<List<Customers>>(jsonData);

            ViewData["address"] = new SelectList(_context.Address, "AddressId", "City");
            return View(model);
        }

        // GET: Admin/Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
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
            var responseProductType = await _client.GetAsync("api/APICustomers/get-Cus");
            var json = await responseProductType.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<IEnumerable<Address>>(json);
            var data = model.Select(e => new SelectListItem
            {
                Text = e.City,
                Value = e.AddressId.ToString()
            }).AsEnumerable();

            ViewData["addresses"] = data;
            return View();
        }
        [HttpPost]
        
        public async Task<IActionResult> Create(Customers customers)
        {
            if (ModelState.IsValid)
            {
                var myContent = JsonConvert.SerializeObject(customers);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byContent = new ByteArrayContent(buffer);
                byContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                await _client.PostAsync("api/APICustomers/add-customer", byContent);
                return RedirectToAction("Index");
            }

            var responseAddress = await _client.GetAsync("api/APICustomers/get-Cus");
            var json = await responseAddress.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<IEnumerable<Address>>(json);
            var data = model.Select(e => new SelectListItem
            {
                Text = e.City,
                Value = e.AddressId.ToString()
            }).AsEnumerable();

            ViewData["addresses"] = data;

            return View(customers);
        }

        // GET: Admin/Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }
            var responseAddress = await _client.GetAsync("api/APICustomers/get-Cus");
            var json = await responseAddress.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<IEnumerable<Address>>(json);
            ViewData["AddressId"] = new SelectList(_context.Address, "AddressId", "City");

            var customers = await _context.Customers.FindAsync(id);
            if (customers == null)
            {
                return NotFound();
            }
            
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
            var responseAddress = await _client.GetAsync("api/APICustomers/get-Cus");
            var json = await responseAddress.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<IEnumerable<Address>>(json);
            ViewData["AddressId"] = new SelectList(_context.Address, "AddressId", "City");

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
            ViewData["AddressId"] = new SelectList(_context.Address, "AddressId", "AddressId", customers.AddressId);
            return View(customers);
        }

        // GET: Admin/Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            ViewData["AddressId"] = new SelectList(_context.Address, "AddressId", "City");
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
            var customers = await _context.Customers.FindAsync(id);
            if (customers != null)
            {
                _context.Customers.Remove(customers);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomersExists(int id)
        {
          return (_context.Customers?.Any(e => e.CustomerId == id)).GetValueOrDefault();
        }
    }
}

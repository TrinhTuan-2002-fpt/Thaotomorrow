using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using PerfumeShop.Models;
using MediaTypeHeaderValue = System.Net.Http.Headers.MediaTypeHeaderValue;

namespace PerfumeShop.Areas.Admin.Controllers.ViewController
{
    [Area("Admin")]
    public class AddressesController : Controller
    {
        private readonly DBContext _context;
        private readonly HttpClient _client = new HttpClient();

        public AddressesController(DBContext context)
        {
            _context = context;
            _client.BaseAddress = new Uri("https://localhost:7015/");
        }

        // GET: Admin/Addresses
        public async Task<IActionResult> Index()
        {
            ViewData["Email"] = HttpContext.Session.GetString("Email");

            var jsonContent = await _client.GetAsync("api/APIAddresses/get-Address");
            var jsonData = await jsonContent.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<List<Address>>(jsonData);
            return View(model);
        }

        // GET: Admin/Addresses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewData["Email"] = HttpContext.Session.GetString("Email");

            if (id == null || _context.Address == null)
            {
                return NotFound();
            }

            var address = await _context.Address
                .FirstOrDefaultAsync(m => m.AddressId == id);
            if (address == null)
            {
                return NotFound();
            }

            return View(address);
        }

        // GET: Admin/Addresses/Create
        public IActionResult Create()
        {
            ViewData["Email"] = HttpContext.Session.GetString("Email");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Address address)
        {
            var add = await _context.Address.Where(c => c.City == address.City).FirstOrDefaultAsync();
            if (add != null)
            {
                ViewData["Add"] = add.City + " Địa chỉ đã có trong hệ thống";
            }
            if (ModelState.IsValid)
            {
                var myContent = JsonConvert.SerializeObject(address);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byContent = new ByteArrayContent(buffer);
                byContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                await _client.PostAsync("api/APIAddresses/add-address", byContent);
                return RedirectToAction("Index");
            }
            return View();
        }

        // GET: Admin/Addresses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewData["Email"] = HttpContext.Session.GetString("Email");

            if (id == null || _context.Address == null)
            {
                return NotFound();
            }

            var address = await _context.Address.FindAsync(id);
            if (address == null)
            {
                return NotFound();
            }
            return View(address);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Address address)
        {
            if (id != address.AddressId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _client.PutAsJsonAsync<Address>($"api/APIAddresses/{id}", address);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AddressExists(address.AddressId))
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
            return View(address);
        }

        // GET: Admin/Addresses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ViewData["Email"] = HttpContext.Session.GetString("Email");

            if (id == null || _context.Address == null)
            {
                return NotFound();
            }

            var address = await _context.Address
                .FirstOrDefaultAsync(m => m.AddressId == id);
            if (address == null)
            {
                return NotFound();
            }

            return View(address);
        }

        // POST: Admin/Addresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Address == null)
            {
                return Problem("Entity set 'DBContext.Address'  is null.");
            }
            var address = await _context.Address.FindAsync(id);
            if (address != null)
            {
                _context.Address.Remove(address);
            }

            await _client.DeleteAsync($"api/APIAddresses/{id}");
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AddressExists(int id)
        {
          return (_context.Address?.Any(e => e.AddressId == id)).GetValueOrDefault();
        }
    }
}

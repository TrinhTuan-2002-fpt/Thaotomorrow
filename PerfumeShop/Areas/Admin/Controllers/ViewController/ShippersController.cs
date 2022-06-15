using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PerfumeShop.Models;

namespace PerfumeShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ShippersController : Controller
    {
        HttpClient client = new HttpClient();
        private DBContext _context;
        public ShippersController(DBContext context)
        {
            _context = context;
            client.BaseAddress = new System.Uri("https://localhost:7015");
        }
        public async Task<IActionResult> Index()
        {
            ViewData["Email"] = HttpContext.Session.GetString("Email");

            var jsonContent = await client.GetAsync("api/ApiShippers/get-list-shipper");
            var jsonData = await jsonContent.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<List<Shippers>>(jsonData);
            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            ViewData["Email"] = HttpContext.Session.GetString("Email");

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Shippers shippers)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var myContent = JsonConvert.SerializeObject(shippers);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            await client.PostAsync("api/ApiShippers/add-shippers", byteContent);
            return RedirectToAction("Index");
            
        }
        public async Task<IActionResult> Edit(int? id)
        {
            ViewData["Email"] = HttpContext.Session.GetString("Email");

            if (id == null || _context.Shippers == null)
            {
                return NotFound();
            }

            var shippers = await _context.Shippers.FirstOrDefaultAsync(c=>c.ShipperId == id);
            if (shippers == null)
            {
                return NotFound();
            }
            return View(shippers);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Shippers shippers, int id)
        {
            if (id != shippers.ShipperId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await client.PutAsJsonAsync<Shippers>($"api/ApiShippers/{id}", shippers);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException e)
                {
                    if (!ShippersExits(shippers.ShipperId))
                    {
                        return NotFound();
                    }
                }
                return RedirectToAction("Index");
            }

            return View(shippers);
        }

        public async Task<IActionResult> Details(int? id)
        {
            ViewData["Email"] = HttpContext.Session.GetString("Email");

            if (id == null || _context.Shippers == null)
            {
                return NotFound();
            }

            var accounts = await _context.Shippers
                .FirstOrDefaultAsync(m => m.ShipperId == id);
            if (accounts == null)
            {
                return NotFound();
            }

            return View(accounts);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            ViewData["Email"] = HttpContext.Session.GetString("Email");

            if (id == null || _context.Shippers == null)
            {
                return NotFound();
            }

            var shippers = await _context.Shippers
                .FirstOrDefaultAsync(m => m.ShipperId == id);
            if (shippers == null)
            {
                return NotFound();
            }

            return View(shippers);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Shippers == null)
            {
                return Problem("Entity set 'DBContext.ProductTypes'  is null.");
            }

            await  client.DeleteAsync($"api/ApiShippers/{id}");
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShippersExits(int id)
        {
            return (_context.Shippers?.Any(c => c.ShipperId == id)).GetValueOrDefault();
        }
    }
}

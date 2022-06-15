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

namespace PerfumeShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductTypeController : Controller
    {
        private readonly DBContext _context;
        private readonly HttpClient _client = new HttpClient();

        public ProductTypeController(DBContext context)
        {
            _context = context;
            _client.BaseAddress= new Uri("https://localhost:7015/");
        }
        
        public async Task<IActionResult> Index()
        {
            ViewData["Email"] = HttpContext.Session.GetString("Email");

            var jsonContent = await _client.GetAsync("api/APIProductTypes/getSP");
            var jsonData = await jsonContent.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<List<ProductTypes>>(jsonData);
            return View(model);
        }
        
        public async Task<IActionResult> Details(int? id)
        {
            ViewData["Email"] = HttpContext.Session.GetString("Email");

            if (id == null || _context.ProductTypes == null)
            {
                return NotFound();
            }

            var productTypes = await _context.ProductTypes
                .FirstOrDefaultAsync(m => m.TypeId == id);
            if (productTypes == null)
            {
                return NotFound();
            }

            return View(productTypes);
        }

        // GET: Admin/ProductType/Create
        
        
        public IActionResult Create()
        {
            ViewData["Email"] = HttpContext.Session.GetString("Email");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductTypes productTypes)
        {
            if (ModelState.IsValid)    
            {
                var myContent = JsonConvert.SerializeObject(productTypes);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byContent = new ByteArrayContent(buffer);
                byContent.Headers.ContentType = new MediaTypeHeaderValue("Application/json");
                await _client.PostAsync("api/APIProductTypes/addSP", byContent);

                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        // GET: Admin/ProductType/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewData["Email"] = HttpContext.Session.GetString("Email");

            if (id == null || _context.ProductTypes == null)
            {
                return NotFound();
            }

            var productTypes = await _context.ProductTypes.FindAsync(id);
            if (productTypes == null)
            {
                return NotFound();
            }
            return View(productTypes);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,ProductTypes productTypes)
        {
            if (id != productTypes.TypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _client.PutAsJsonAsync<ProductTypes>($"api/APIProductTypes/{id}",productTypes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductTypesExists(productTypes.TypeId))
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
            return View(productTypes);
        }
        
        public async Task<IActionResult> Delete(int? id)
        {
            ViewData["Email"] = HttpContext.Session.GetString("Email");

            if (id == null || _context.ProductTypes == null)
            {
                return NotFound();
            }

            var productTypes = await _context.ProductTypes
                .FirstOrDefaultAsync(m => m.TypeId == id);
            if (productTypes == null)
            {
                return NotFound();
            }

            return View(productTypes);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ProductTypes == null)
            {
                return Problem("Entity set 'DBContext.ProductTypes'  is null.");
            }

            await _client.DeleteAsync($"api/APIProductTypes/{id}");
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductTypesExists(int id)
        {
          return (_context.ProductTypes?.Any(e => e.TypeId == id)).GetValueOrDefault();
        }
    }
}

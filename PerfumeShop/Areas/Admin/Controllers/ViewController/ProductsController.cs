using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PerfumeShop.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace PerfumeShop.Areas.Admin.Controllers.ViewController
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly DBContext _context;
        private  HttpClient client = new HttpClient();

        public ProductsController(DBContext context)
        {
            client.BaseAddress = new Uri("https://localhost:7015/");
            _context = context;
        }

        // GET: Admin/Accounts
        public async Task<IActionResult> Index()
        {
            var jsonConnect = client.GetAsync("api/ApiProducts/Get-All").Result;
            string jsonData = jsonConnect.Content.ReadAsStringAsync().Result;

            //Lay list tu API
            var model = JsonConvert.DeserializeObject<List<Products>>(jsonData);

            ViewData["TypeId"] = new SelectList(_context.ProductTypes, "TypeId", "Name");
            ViewData["FragrantId"] = new SelectList(_context.Fragrant, "FragrantId", "Name");
            return View(model);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

           
            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.ProdcutId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
        public async Task<IActionResult> Create()
        {
            var responseProductType = await client.GetAsync("api/APIProductTypes/getSP");
            var json = await responseProductType.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<IEnumerable<ProductTypes>>(json);
            var data = model.Select(e => new SelectListItem
            {
                Text = e.Name,
                Value = e.TypeId.ToString()
            }).AsEnumerable();

            ViewData["types"] = data;
            ViewData["FragrantId"] = new SelectList(_context.Fragrant, "FragrantId", "Name");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Products products)
        {

            if (ModelState.IsValid)
            {
                var myContent = JsonConvert.SerializeObject(products);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byContent = new ByteArrayContent(buffer);
                byContent.Headers.ContentType = new MediaTypeHeaderValue("Application/json");
                await client.PostAsync("api/ApiProducts/addSP", byContent);

                return RedirectToAction(nameof(Index));
            }

            var responseProductType = await client.GetAsync("api/APIProductTypes/getSP");
            var json = await responseProductType.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<IEnumerable<ProductTypes>>(json);
            var data = model.Select(e => new SelectListItem
            {
                Text = e.Name,
                Value = e.TypeId.ToString()
            }).AsEnumerable();

            ViewData["types"] = data;
            ViewData["FragrantId"] = new SelectList(_context.Fragrant, "FragrantId", "Name");

            return View(products);
        }
        public async Task<IActionResult> Edit(int? id)
        {
           
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }
            var responseProductType = await client.GetAsync("api/APIProductTypes/getSP");
            var json = await responseProductType.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<IEnumerable<ProductTypes>>(json);
            ViewData["TypeId"] = new SelectList(_context.ProductTypes, "TypeId", "Name");
            ViewData["FragrantId"] = new SelectList(_context.Fragrant, "FragrantId", "Name");


            var products = await _context.Products.FindAsync(id);
            if (products == null)
            {
                return NotFound();
            }
            return View(products);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Products products)
        {
            if (id != products.ProdcutId)
            {
                return NotFound();
            }
            var responseProductType = await client.GetAsync("api/APIProductTypes/getSP");
            var json = await responseProductType.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<IEnumerable<ProductTypes>>(json);
            ViewData["TypeId"] = new SelectList(_context.ProductTypes, "TypeId", "Name");
            ViewData["FragrantId"] = new SelectList(_context.Fragrant, "FragrantId", "Name");
            if (ModelState.IsValid)
            {
                try
                {
                  
                    await client.PutAsJsonAsync<Products>($"api/ApiProducts/{id}", products);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if(!ProductsExists(products.ProdcutId))
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
            return View(products);
        }
        

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products== null)
            {
                return NotFound();
            }
            ViewData["TypeId"] = new SelectList(_context.ProductTypes, "TypeId", "Name");
            ViewData["FragrantId"] = new SelectList(_context.Fragrant, "FragrantId", "Name");
            var products = await _context.Products
                .FirstOrDefaultAsync(m => m.ProdcutId == id);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'DBContext.Products'  is null.");
            }

            await client.DeleteAsync($"api/ApiProducts/{id}");
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductsExists(int id)
        {
            return (_context.Products?.Any(e => e.ProdcutId == id)).GetValueOrDefault();
        }
    }
}

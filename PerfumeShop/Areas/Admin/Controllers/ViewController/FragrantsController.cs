using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PerfumeShop.Models;
using System.Net.Http.Headers;

namespace PerfumeShop.Areas.Admin.Controllers.ViewController
{
    [Area("Admin")]
    public class FragrantsController : Controller
    {
        private readonly DBContext _context;
        private readonly HttpClient _client = new HttpClient();
        
        public FragrantsController(DBContext context)
        {
            _context = context;
            _client.BaseAddress = new Uri("https://localhost:7015/");
        }

        public async Task<IActionResult> Index()
        {
            var jsonContent = await _client.GetAsync("api/ApiFragrants/get-Fragrants");
            var jsonData = await jsonContent.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<List<Fragrant>>(jsonData);
            return View(model);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Fragrant == null)
            {
                return NotFound();
            }

            var fragrant = await _context.Fragrant
                .FirstOrDefaultAsync(m => m.FragrantId == id);
            if (fragrant == null)
            {
                return NotFound();
            }

            return View(fragrant);
        }

        // GET: Admin/ProductType/Create


        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Fragrant fragrant)
        {
            if (ModelState.IsValid)
            {
                var myContent = JsonConvert.SerializeObject(fragrant);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byContent = new ByteArrayContent(buffer);
                byContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                await _client.PostAsync("api/ApiFragrants/add-fragrant", byContent);

                return RedirectToAction("Index");
            }

            return View();
        }
        // GET: Admin/ProductType/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Fragrant == null)
            {
                return NotFound();
            }

            var fragrant = await _context.Fragrant.FindAsync(id);
            if (fragrant == null)
            {
                return NotFound();
            }
            return View(fragrant);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Fragrant fragrant)
        {
            if (id != fragrant.FragrantId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _client.PutAsJsonAsync<Fragrant>($"api/ApiFragrants/{id}", fragrant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FragrantExists(fragrant.FragrantId))
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
            return View(fragrant);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Fragrant == null)
            {
                return NotFound();
            }

            var fragrant = await _context.Fragrant
                .FirstOrDefaultAsync(m => m.FragrantId == id);
            if (fragrant == null)
            {
                return NotFound();
            }

            return View(fragrant);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Fragrant == null)
            {
                return Problem("Entity set 'DBContext.Fragrant'  is null.");
            }

            await _client.DeleteAsync($"api/ApiFragrants/{id}");
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool FragrantExists(int id)
        {
            return (_context.Fragrant?.Any(e => e.FragrantId == id)).GetValueOrDefault();
        }

    }
}


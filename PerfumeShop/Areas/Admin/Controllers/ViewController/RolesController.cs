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

namespace PerfumeShop.Areas.Admin.ControllersView
{
    [Area("Admin")]
    public class RolesController : Controller
    {
        private readonly DBContext _context;
        private HttpClient client = new HttpClient();

        public RolesController(DBContext context)
        {
            client.BaseAddress = new Uri("https://localhost:7015/");
            _context = context;
        }

        // GET: Admin/Roles
        public async Task<IActionResult> Index()
        {
            ViewData["Email"] = HttpContext.Session.GetString("Email");

            var jsonConnect = client.GetAsync("api/Role/API-Get-List-Roles").Result;
            string jsonData = jsonConnect.Content.ReadAsStringAsync().Result;

            //Lay list tu API
            var model = JsonConvert.DeserializeObject<List<Roles>>(jsonData);
            return View(model);
        }

        // GET: Admin/Roles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewData["Email"] = HttpContext.Session.GetString("Email");

            if (id == null || _context.Roles == null)
            {
                return NotFound();
            }

            var roles = await _context.Roles
                .FirstOrDefaultAsync(m => m.RoleId == id);
            if (roles == null)
            {
                return NotFound();
            }

            return View(roles);
        }

        // GET: Admin/Roles/Create
        public IActionResult Create()
        {
            ViewData["Email"] = HttpContext.Session.GetString("Email");

            return View();
        }

        // POST: Admin/Roles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Roles roles)
        {
            var acc = await _context.Roles.Where(c => c.Name == roles.Name).FirstOrDefaultAsync();
            if (acc != null)
            {
                ViewData["Name"] = acc.Name + " Đã có trong hệ thống";
                return View();
            }
            if (ModelState.IsValid)
            {
                var myContent = JsonConvert.SerializeObject(roles);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byContent = new ByteArrayContent(buffer);
                byContent.Headers.ContentType = new MediaTypeHeaderValue("Application/json");
                await client.PostAsync("api/Role/API-Add-Roles", byContent);
                return RedirectToAction(nameof(Index));
            }
            return View(roles);
        }

        // GET: Admin/Roles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewData["Email"] = HttpContext.Session.GetString("Email");

            if (id == null || _context.Roles == null)
            {
                return NotFound();
            }

            var roles = await _context.Roles.FindAsync(id);
            if (roles == null)
            {
                return NotFound();
            }
            return View(roles);
        }

        // POST: Admin/Roles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Roles roles)
        {
            if (id != roles.RoleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // _context.Update(roles);
                    await client.PutAsJsonAsync<Roles>($"api/Role/{id}", roles);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RolesExists(roles.RoleId))
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
            return View(roles);
        }

        // GET: Admin/Roles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ViewData["Email"] = HttpContext.Session.GetString("Email");

            if (id == null || _context.Roles == null)
            {
                return NotFound();
            }

            var roles = await _context.Roles
                .FirstOrDefaultAsync(m => m.RoleId == id);
            if (roles == null)
            {
                return NotFound();
            }

            return View(roles);
        }

        // POST: Admin/Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Roles == null)
            {
                return Problem("Entity set 'DBContext.Roles'  is null.");
            }

            await client.DeleteAsync($"api/Role/{id}");
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RolesExists(int id)
        {
          return (_context.Roles?.Any(e => e.RoleId == id)).GetValueOrDefault();
        }
    }
}

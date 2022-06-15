using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PerfumeShop.Models;

namespace PerfumeShop.Controllers.ViewController
{
    public class CustomersController : Controller
    {
        private readonly DBContext _context;
        private readonly HttpClient _client = new HttpClient();
        public CustomersController(DBContext context)
        {
            _context = context;
            _client.BaseAddress = new Uri("https://localhost:7015/");
        }
        [HttpGet]
        public IActionResult Login(Customers customer)
        {
            return View();
        }
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(Customers model)
        {
            if (ModelState.IsValid)
            {
                var myContent = JsonConvert.SerializeObject(model);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byContent = new ByteArrayContent(buffer);
                byContent.Headers.ContentType = new MediaTypeHeaderValue("Application/json");
                await _client.PostAsync("api/ApiCustomers", byContent);
                return RedirectToAction(nameof(Login));
            }
            return RedirectToAction(nameof(Login));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            Customers customer = new Customers();
            if (!String.IsNullOrEmpty(email) && !String.IsNullOrEmpty(password))
            {
                customer = await _context.Customers.FirstOrDefaultAsync(c =>
                    c.Email == email && c.Password == password);
                if (customer != null)
                {
                    HttpContext.Session.SetString("email", email);
                    HttpContext.Session.SetString("password", password);
                    HttpContext.Session.SetString("name", customer.Name);
                    return RedirectToAction("Index", "Home");
                }

                ViewData["Erorr"] = "Tên tài khoản hoặc mật khẩu không chính xác!";
            }

            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("name");
            return RedirectToAction("Index", "Home");
        }
    }
}

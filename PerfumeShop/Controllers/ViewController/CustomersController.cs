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
        private readonly HttpClient _client;
        public CustomersController(DBContext context)
        {
            _client = new HttpClient();
            _context = context;
            _client.BaseAddress = new Uri("https://localhost:7015/");
        }
        public Customers Search(string email, string password)
        {
            var jsonContent =  _client.GetAsync("api/APICustomers/login").Result;
            var jsonData =  jsonContent.Content.ReadAsStringAsync().Result;
            var model = JsonConvert.DeserializeObject<Customers>(jsonData);
            return model;
        }

        public async Task<IActionResult> Login(string email, string password)
        {
            Customers customer = new Customers();
            if (!String.IsNullOrWhiteSpace(email) && !String.IsNullOrWhiteSpace(password))
            {
                customer = await _context.Customers.FirstOrDefaultAsync(c =>
                    c.Email == email && c.Password == password);
                if (customer != null)
                {
                    Search(email, password);
                    HttpContext.Session.SetString("email", email);
                    HttpContext.Session.SetString("password", password);
                    HttpContext.Session.SetString("name", customer.Name);
                    HttpContext.Session.SetString("idcus", customer.CustomerId.ToString());
                    return RedirectToAction("Index", "Home");
                }

                ViewData["Erorr"] = "Tên tài khoản hoặc mật khẩu không chính xác!";
            }

            return View();
        }

        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(Customers model)
        {
            if (ModelState.IsValid)
            {
                var myContent = JsonConvert.SerializeObject(model);
                var res = new StringContent(myContent, System.Text.Encoding.UTF8, "application/json");
                await _client.PostAsync("api/ApiCustomers", res);
                return RedirectToAction(nameof(Login));
            }
            return RedirectToAction(nameof(Login));
        }
        

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("name");
            return RedirectToAction("Index", "Home");
        }
    }
}

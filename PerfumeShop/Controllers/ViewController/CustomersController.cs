using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PerfumeShop.Models;

namespace PerfumeShop.Controllers.ViewController
{
    public class CustomersController : Controller
    {
        private readonly DBContext _context;

        public CustomersController(DBContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Login(Customers customer)
        {
            return View();
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

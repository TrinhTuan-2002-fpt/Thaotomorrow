using Microsoft.AspNetCore.Mvc;
using PerfumeShop.Models;

namespace PerfumeShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly DBContext _context;

        public HomeController(DBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            ViewData["Email"] = HttpContext.Session.GetString("Email");
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string Username, string Password)
        {
            Accounts _account;
            if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password))
            {
                _account = _context.Accounts.FirstOrDefault(p => p.Email == Username && p.Password == Password);
                if (_account != null)
                {
                    HttpContext.Session.SetString("Email", _account.Name);
                    HttpContext.Session.SetString("Password", Password);
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewData["Error"] = "Tài Khoản Hoặc Mật Khẩu Sai Và Status Là HĐ Mới Login Được";
                }
            }

            return View();
        }
        public IActionResult LogOut()
        {
            HttpContext.Session.Remove("Email");
            return RedirectToAction(nameof(Index));
        }
    }
}

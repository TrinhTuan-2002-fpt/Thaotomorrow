using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PerfumeShop.Models;

namespace PerfumeShop.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly DBContext _context;

    public HomeController(ILogger<HomeController> logger, DBContext context)
    {
        _logger = logger;
        _context = context;
    }
    public IActionResult Index()
    {
        ViewData["name"] = HttpContext.Session.GetString("name");
        // ViewData["ProductType"] = _context.ProductTypes.ToList();
        // ViewData["Product"] = _context.Products.ToList();
        // ViewData["Cart"] = _context.Orders.ToList();
        return View();
    }
    public IActionResult Contact()
    {
        // ViewData["Email"] = HttpContext.Session.GetString("Email");
        // ViewData["ProductType"] = _context.ProductTypes.ToList();
        // ViewData["Product"] = _context.Products.ToList();
        // ViewData["Cart"] = _context.Orders.ToList();
        return View();
    }
    public IActionResult About()
    {
        // ViewData["Email"] = HttpContext.Session.GetString("Email");
        // ViewData["ProductType"] = _context.ProductTypes.ToList();
        // ViewData["Product"] = _context.Products.ToList();
        // ViewData["Cart"] = _context.Orders.ToList();
        return View();
    }
    public IActionResult ViewCart()
    {
        // ViewData["Email"] = HttpContext.Session.GetString("Email");
        // ViewData["ProductType"] = _context.ProductTypes.ToList();
        // ViewData["Product"] = _context.Products.ToList();
        // var cartTotal = _context.Orders.ToList();
        // ViewData["Cart"] = cartTotal;
        // if (cartTotal.Count() > 0)
        // {
        //     ViewData["abc"] = cartTotal.Sum(c => c.Product.Price);
        //
        // }

        return View();
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
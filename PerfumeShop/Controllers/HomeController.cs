using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PerfumeShop.Models;
using PerfumeShop.ModelView;

namespace PerfumeShop.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly DBContext _context;
    private readonly HttpClient _httpClient = new HttpClient();
    public HomeController(ILogger<HomeController> logger, DBContext context)
    {
        _logger = logger;
        _context = context;
        _httpClient.BaseAddress = new Uri("https://localhost:7015/");
    }
    public IActionResult Index()
    {
        ViewData["name"] = HttpContext.Session.GetString("name");
        ViewData["idCus"] = HttpContext.Session.GetString("idcus");
        ViewData["ProductType"] = _context.ProductTypes.ToList();
        ViewData["Product"] = _context.Products.ToList();
        return View(_context.Products);
    }
    public IActionResult Contact()
    {
        ViewData["name"] = HttpContext.Session.GetString("name");
        ViewData["ProductType"] = _context.ProductTypes.ToList();
        return View();
    }
    public IActionResult About()
    {
        ViewData["name"] = HttpContext.Session.GetString("name");
        ViewData["ProductType"] = _context.ProductTypes.ToList();
        return View();
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
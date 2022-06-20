using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PerfumeShop.Models;
using PerfumeShop.ModelView;

namespace PerfumeShop.Controllers.ViewController
{
    public class CartController : Controller
    {
        private readonly DBContext _context;
        private readonly HttpClient _httpClient =new HttpClient();
        public CartController(DBContext context)
        {
            _context = context;
            _httpClient.BaseAddress = new Uri("https://localhost:7015/");
        }
       
        public async Task<IActionResult> ViewCart()
        {
            var jsonConnect = await _httpClient.GetAsync("api/ApiCartDetails");
            string jsonData = await jsonConnect.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<CartModel>(jsonData);
            ViewData["name"] = HttpContext.Session.GetString("name");
            return View(model);
        }

        [Route("add/{user}/{id}")]
        public async Task<IActionResult> AddtoCart(string user,int id)
        {
            var response = await _httpClient.PostAsync($"api/Cart/{user}/items/{id}", null);
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                return NotFound();
            return RedirectToAction("ViewCart", "Cart");
        }
        [Route("remove/{id}")]
        public async Task<IActionResult> Remove(int idcart,int id)
        {
            var response = await _httpClient.DeleteAsync($"api/Cart/{idcart}/items/{id}");

            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                return NotFound();

            return RedirectToAction("ViewCart", "Cart");

        }
    }
}

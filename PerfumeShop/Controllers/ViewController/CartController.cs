using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PerfumeShop.Helpers;
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
            var CusId = HttpContext.Session.GetString("idcus");
            var jsonConnect = await _httpClient.GetAsync($"api/ApiCartDetails/{CusId}");
            string jsonData = await jsonConnect.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<CartModel>(jsonData);
            ViewData["name"] = HttpContext.Session.GetString("name");
            return View(model);
        }

        [Route("add/{user}/{id}")]
        public async Task<IActionResult> AddtoCart(string user,int id)
        {
            var response = await _httpClient.PostAsync($"api/Cart/{user}/items/{id}", null);
            var CusId = HttpContext.Session.GetString("idCus");
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                return NotFound();
            return RedirectToAction("ViewCart", "Cart");
            //return RedirectToAction("Index", "Home");
        }
        [Route("remove/{id}")]
        public async Task<IActionResult> Remove(int idcart,int id)
        {
            var response = await _httpClient.DeleteAsync($"api/Cart/{idcart}/items/{id}");

            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                return NotFound();

            return RedirectToAction("ViewCart", "Cart");

        }
        [Route("reduce/{id}")]
        public async Task<IActionResult> Reduce(int idcart,int id)
        {
            var response = await _httpClient.PatchAsync($"api/Cart/{idcart}/items/{id}/reduce", null);

            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                return NotFound();

            return RedirectToAction("ViewCart", "Cart");
        }

        [Route("increase/{id}")]
        public async Task<IActionResult> Increase(int idcart,int id)
        {
            var response = await _httpClient.PatchAsync($"api/Cart/{idcart}/items/{id}/increase", null);

            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                return NotFound();

            return RedirectToAction("ViewCart", "Cart");
        }
        [Route("checkout")]
        public async Task<IActionResult> CheckOut()
        {
            var CusId = HttpContext.Session.GetString("idcus");
            _ = await _httpClient.PatchAsync($"api/Cart/{CusId}", null);
            return RedirectToAction("Index", "Home");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PerfumeShop.Models;
using PerfumeShop.ModelView;

namespace PerfumeShop.Controllers.ApiController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiCartDetailsController : ControllerBase
    {
        private readonly DBContext _context;

        public ApiCartDetailsController(DBContext context)
        {
            _context = context;
        }

        // GET: api/CartDetails
        [HttpGet]
        public async Task<IActionResult> GetCartDetails()
        {
            var cart = await _context.Carts.Include(c => c.CartDetails)
                .ThenInclude(c=>c.Products)
                .FirstOrDefaultAsync();
            var result = new CartModel
            {
                Id = cart.CartId,
                Total = cart.Total,
                Items = cart.CartDetails.Select(e => new CartItemModel
                {
                    Id = e.CartId,
                    ProductId = e.ProductId,
                    Total = e.Payment,
                    Image = e.Products.Img,
                    Name = e.Products.Name,
                    Quantity = e.Amount,
                    Price = e.Products.Price,
                    Max = e.Products.Amount
                })
            };
            
            return Ok(result);
        }
    }
}

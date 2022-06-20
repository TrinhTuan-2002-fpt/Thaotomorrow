using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PerfumeShop.Models;

namespace PerfumeShop.Controllers.ApiController
{
    [ApiController]
    [Route("api/Cart")]
    public class ApiCartsController : ControllerBase
    {
        private readonly DBContext _context;

        public ApiCartsController(DBContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("{user}/items/{id:int}")]
        public async Task<ActionResult<Carts>> PostCarts([FromRoute] string user, int id)
        {
            var cus = await _context.Customers.FindAsync(Convert.ToInt32(user));
            var product = await _context.Products.FindAsync(id);
            var cart = await _context.Carts.
                Include(c => c.CartDetails)
                .FirstOrDefaultAsync(c=> c.CustomerId == Convert.ToInt32(user));
            var listShipper = _context.Shippers.Where(c => c.Status == 1).ToList();
            if (cart == null)
                cart = new Carts();
            if (product != null) return BadRequest();
            var Cartdetail = cart.CartDetails?.FirstOrDefault(c => c.ProductId == id);
            if (Cartdetail != null)
            {
                Cartdetail.Amount++;
                Cartdetail.Payment = product.Price * Cartdetail.Amount;
                cart.Total += product.Price;
                _context.Carts.Update(cart);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            if (cart.CartId == 0)
            {
                cart.Oderdate = DateTime.Now;
                cart.Shipdate = DateTime.Now.AddDays(5);
                cart.ShipperId = Random.Shared.Next(listShipper.Count);
                cart.CustomerId = cus.CustomerId;
                cart.CartDetails = new List<CartDetails>()
                {
                    new CartDetails {ProductId = product.ProdcutId, Payment = product.Price, Amount = 1}
                };
                _context.Carts.Update(cart);
                await _context.Carts.AddAsync(cart);
                await _context.SaveChangesAsync();
                return NoContent();
            }

            var Detail = new CartDetails {CartId = cart.CartId, Payment = product.Price, Amount = 1, ProductId = product.ProdcutId};
            cart.Total += Detail.Payment;
            _context.CartDetails.Update(Detail);
            await _context.CartDetails.AddAsync(Detail);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}

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
        private Random _radom = new Random();

        public ApiCartsController(DBContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("{user}/items/{id:int}")]
        public async Task<ActionResult<Carts>> PostCarts([FromRoute] string user, int id)
        {
            var product = await _context.Products.FindAsync(id);
            var cart = await _context.Carts.
                Include(c => c.CartDetails)
                .FirstOrDefaultAsync(c=> c.CustomerId == Convert.ToInt32(user) && !c.Status);
            var listShipper = _context.Shippers.Where(c => c.Status == 1).ToList();
            var ShipId = _radom.Next(listShipper.Count);
            if (cart == null)
                cart = new Carts();
            if (product == null) return BadRequest();
            var Cartdetail = cart.CartDetails?.FirstOrDefault(c => c.ProductId == id);
            if (cart.CartId == 0)
            {

                cart.Oderdate = DateTime.Now;
                cart.Shipdate = DateTime.Now.AddDays(5);
                cart.ShipperId = listShipper[ShipId].ShipperId;
                cart.CustomerId = Convert.ToInt32(user);
                cart.CartDetails = new List<CartDetails>()
                {
                    new CartDetails {ProductId = product.ProdcutId, Payment = product.Price, Amount = 1}
                };
                cart.Total = product.Price;
                await _context.Carts.AddAsync(cart);
                //_context.Carts.Update(cart);
                
                await _context.SaveChangesAsync();
                return NoContent();
            }
            if (Cartdetail != null)
            {
                Cartdetail.Amount++;
                Cartdetail.Payment = product.Price * Cartdetail.Amount;
                cart.Total += product.Price;
                _context.Carts.Update(cart);
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
        [HttpDelete]
        [Route("{idcart}/items/{id:int}")]
        public async Task<IActionResult> RemoveItem([FromRoute] int idcart, int id)
        {
            var item = await _context.CartDetails.Where(c=>c.ProductId == id).FirstOrDefaultAsync();

            if (item == null)
                return BadRequest();

            var cart = await _context.Carts.FirstOrDefaultAsync(e => e.CartId == idcart);

            if (cart == null)
                return BadRequest();

            cart.Total -= item.Payment;

            _context.Carts.Update(cart);
            _context.CartDetails.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpPatch]
        [Route("{idcart}/items/{id:int}/reduce")]
        public async Task<IActionResult> ReduceQuantity([FromRoute] int idcart, int id)
        {
            var item = await _context.CartDetails.FirstOrDefaultAsync(c => c.CartId == idcart && c.ProductId == id);

            if (item == null)
                return BadRequest();

            var cart = await _context.Carts.Include(e => e.CartDetails)
                .FirstOrDefaultAsync(e => e.CartId == idcart);

            if (cart == null)
                return BadRequest();

            var product = await _context.Products.FirstOrDefaultAsync(e => e.ProdcutId == item.ProductId);

            if (product == null)
                return BadRequest();

            item.Amount--;
            item.Payment -= product.Price;
            cart.Total -= product.Price;
            _context.CartDetails.Update(item);
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch]
        [Route("{idcart}/items/{id:int}/increase")]
        public async Task<IActionResult> IncreaseQuantity([FromRoute] int idcart, int id)
        {
            var item = await _context.CartDetails.FirstOrDefaultAsync(c=>c.CartId == idcart && c.ProductId == id);

            if (item == null)
                return BadRequest();

            var cart = await _context.Carts.Include(e => e.CartDetails)
                .FirstOrDefaultAsync(e => e.CartId == idcart);

            if (cart == null)
                return BadRequest();

            var product = await _context.Products.FirstOrDefaultAsync(e => e.ProdcutId == item.ProductId);

            if (product == null)
                return BadRequest();

            
            if (item.Amount == product.Amount)
                return NoContent();
            item.Amount++;

            item.Payment += product.Price;
            cart.Total += product.Price;
            _context.CartDetails.Update(item);
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpPatch]
        [Route("{customerId}")]
        public async Task<IActionResult> CheckOut([FromRoute] string customerId)
        {
            var cart = await _context.Carts
                .Include(e => e.CartDetails)
                .FirstOrDefaultAsync(e => !e.Status && e.CustomerId == Convert.ToInt32(customerId));
            if (cart == null || cart.CartDetails == null)
            {
                return BadRequest();
            }

            cart.Status = true;
            foreach (var x in cart.CartDetails)
            {
                var product = await _context.Products.FindAsync(x.ProductId);

                if (product == null)
                    return BadRequest();

                product.Amount -= x.Amount;

                _context.Products.Update(product);
            }

            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

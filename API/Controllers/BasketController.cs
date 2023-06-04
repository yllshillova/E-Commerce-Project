using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class BasketController : BaseApiController
    {
        private readonly StoreContext _context;
        public BasketController(StoreContext context)
        {
            _context = context;
        }

        [HttpGet(Name = "GetBasket")]
        public async Task<ActionResult<BasketDto>> GetBasket()
        {
            // per me e kthy basket na duhet me bo include items per arsyje se perbehet prej basket items te ndryshem,
            // poashtu duhet te ja bashkangjitim edhe produktin si pjese e basketItem dhe ne fund ta kthejm at shporte bazuar ne id e bleresit.
            var basket = await RetrieveBasket(GetBuyerId());
            if (basket == null) return NotFound();
            return basket.MapBasketToDto();

        }



        [HttpPost] // api/basket?productId=3&quantity=2
        public async Task<ActionResult<BasketDto>> AddItemToBasket(int productId, int quantity)
        {
            // get basket || create basket(if the user doesnt have a basket)
            var basket = await RetrieveBasket(GetBuyerId());
            if (basket == null) basket = CreateBasket();

            // get product                    // findAsync perdoret per me gjet diqka ne nje tabel psh products e gjen ne baz te id qe ja qojm si parameter
            var product = await _context.Products.FindAsync(productId);
            if (product == null) return BadRequest(new ProblemDetails { Title = "Product not found" });

            // add item
            basket.AddItem(product, quantity);

            // save changes

            //savechangesasync kthen nje numer te ndryshimeve qe jon rujt e per qata bohet krahasimi qe me kqyr a ubo naj ndryshim ndatabaz,
            //nese jo ather ka fail edhe e kthejm nje bad request.
            var result = await _context.SaveChangesAsync() > 0;
            // kjo na kthen 201 response qysh duhet dmth bashk me nje location header
            if (result) return CreatedAtRoute("GetBasket", basket.MapBasketToDto());
            return BadRequest(new ProblemDetails { Title = "Problem saving item to basket" });
        }


        [HttpDelete]
        public async Task<ActionResult> RemoveBasketItem(int productId, int quantity)
        {
            // get basket
            var basket = await RetrieveBasket(GetBuyerId());
            if (basket == null) return NotFound();
            // remove item or reduce quantity
            basket.RemoveItem(productId, quantity);
            // save changes
            var result = await _context.SaveChangesAsync() > 0;
            if (result) return Ok();
            return BadRequest(new ProblemDetails { Title = "Problem removing item from the basket" });
        }

        private async Task<Basket> RetrieveBasket(string buyerId)
        {
            if (string.IsNullOrEmpty(buyerId))
            {
                Response.Cookies.Delete("buyerId");
                return null;
            }

            return await _context.Baskets
                .Include(i => i.Items)
                .ThenInclude(p => p.Product)
                .FirstOrDefaultAsync(x => x.BuyerId == buyerId);
        }
        private string GetBuyerId()
        {
            // kjo '??' nenkupton nese osht null pjesa e par pra user.identity.name ekzekutohet pjesa ne te djatht request.cookies['buyerid'].
            return User.Identity?.Name ?? Request.Cookies["buyerId"];
        }
        private Basket CreateBasket()
        {
            // nese jon logged in e vendosum buyerid si emri kurse nese so e vendosim si guid edhe punojm me anonymous basket 
            var buyerId = User.Identity?.Name;
            if (string.IsNullOrEmpty(buyerId))
            {
                buyerId = Guid.NewGuid().ToString();
                // krijimi i nje cookie
                var cookieOptions = new CookieOptions { IsEssential = true, Expires = DateTime.Now.AddDays(30) };
                // shtimi i cookie ne fjale
                Response.Cookies.Append("buyerId", buyerId, cookieOptions);
            }
            // krijimi i shportes
            var basket = new Basket { BuyerId = buyerId };
            _context.Baskets.Add(basket);
            return basket;
        }
     

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
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

        [HttpGet]
        public async Task<ActionResult<Basket>> GetBasket()
        {
            // per me e kthy basket na duhet me bo include items per arsyje se perbehet prej basket items te ndryshem,
            // poashtu duhet te ja bashkangjitim edhe produktin si pjese e basketItem dhe ne fund ta kthejm at shporte bazuar ne id e bleresit.
            Basket basket = await RetrieveBasket();
            if (basket == null) return NotFound();
            return basket;

        }


        [HttpPost] // api/basket?productId=3&quantity=2
        public async Task<ActionResult> AddItemToBasket(int productId, int quantity)
        {
            // get basket || create basket(if the user doesnt have a basket)
            var basket = await RetrieveBasket();
            if (basket == null) basket = CreateBasket();

            // get product                    // findAsync perdoret per me gjet diqka ne nje tabel psh products e gjen ne baz te id qe ja qojm si parameter
            var product = await _context.Products.FindAsync(productId);
            if(product == null) return NotFound();

            // add item
            basket.AddItem(product, quantity);

            // save changes

            //savechangesasync kthen nje numer te ndryshimeve qe jon rujt e per qata bohet krahasimi qe me kqyr a ubo naj ndryshim ndatabaz,
            //nese jo ather ka fail edhe e kthejm nje bad request.
            var result = await _context.SaveChangesAsync() > 0;
            if(result) return StatusCode(201);
            return BadRequest(new ProblemDetails{Title= "Problem saving item to basket"});
        }


        [HttpDelete]
        public async Task<ActionResult> RemoveBasketItem(int productId, int quantity)
        {
            // get basket
            // remove item or reduce quantity
            // save changes
            return Ok();
        }

        private async Task<Basket> RetrieveBasket()
        {
            return await _context.Baskets
                .Include(i => i.Items)
                .ThenInclude(p => p.Product)
                .FirstOrDefaultAsync(x => x.BuyerId == Request.Cookies["buyerId"]);
        }
        private Basket CreateBasket()
        {
            // guid reprezenton nje unique id
            var buyerId = Guid.NewGuid().ToString();
            // krijimi i nje cookie
            var cookieOptions = new CookieOptions{IsEssential = true, Expires = DateTime.Now.AddDays(30)};
            // shtimi i cookie ne fjale
            Response.Cookies.Append("buyerId",buyerId,cookieOptions);
            // krijimi i shportes
            var basket = new Basket{BuyerId = buyerId};
            _context.Baskets.Add(basket);
            return basket;
        }


    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Entities.OrderAgregate;
using API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    public class OrdersController : BaseApiController
    {
        private readonly StoreContext _context;
        public OrdersController(StoreContext context)
        {
            _context = context;

        }

        [HttpGet]
        public async Task<ActionResult<List<OrderDto>>> GetOrders()
        {
            return await _context.Orders
                .ProjectOrderToOrderDto()
                .Where(x => x.BuyerId == User.Identity.Name)
                .ToListAsync();
        }

        [HttpGet("{id}", Name = "GetOrder")]
        public async Task<ActionResult<OrderDto>> GetOrder(int id)
        {
            return await _context.Orders
                .ProjectOrderToOrderDto()
                .Where(x => x.BuyerId == User.Identity.Name && x.Id == id)
                .FirstOrDefaultAsync();
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateOrder(CreateOrderDto orderDto)
        {
            // Merr shportën për përdoruesin aktual
            var basket = await _context.Baskets
                .RetrieveBasketWithItems(User.Identity.Name)
                .FirstOrDefaultAsync();

            // Kontrollo nëse nuk gjendet shporta
            if (basket == null)
            {
                return BadRequest(new ProblemDetails { Title = "Could not locate basket" });
            }

            // Krijo listën e artikujve të porosisë
            var items = new List<OrderItem>();

            // Për çdo artikull në shportë
            foreach (var item in basket.Items)
            {
                // Gjej produktin përkatës në bazën e të dhënave
                var productItem = await _context.Products.FindAsync(item.ProductId);

                // Krijo informacionin e produktit të porositur
                var itemOrdered = new ProductItemOrdered
                {
                    ProductId = productItem.Id,
                    Name = productItem.Name,
                    PictureUrl = productItem.PictureUrl
                };

                // Krijo artikullin e porosisë
                var orderItem = new OrderItem
                {
                    ItemOrdered = itemOrdered,
                    Price = productItem.Price,
                    Quantity = item.Quantity
                };

                // Shto artikullin në listë
                items.Add(orderItem);

                // Përditëso sasinë në depo të produktit
                productItem.QuantityInStock -= item.Quantity;
            }

            // Llogaritë subtotalen e porosisë
            var subtotal = items.Sum(item => item.Price * item.Quantity);

            // Cakto tarifën e dërgesës së mallrave
            var deliveryFee = subtotal > 10000 ? 0 : 500;

            // Krijo porosinë
            var order = new Order
            {
                OrderItems = items,
                BuyerId = User.Identity.Name,
                ShippingAddress = orderDto.ShippingAddress,
                Subtotal = subtotal,
                DeliveryFee = deliveryFee
            };

            // Shto porosinë në bazën e të dhënave
            _context.Orders.Add(order);

            // Fshij shportën nga bazë e të dhënave
            _context.Baskets.Remove(basket);

            // Nëse përdoruesi dëshiron të ruajë adresën, atëherë ruaj atë në bazën e të dhënave
            if (orderDto.SaveAddress)
            {
                var user = await _context.Users
                .Include(a => a.Address)
                .FirstOrDefaultAsync(x => x.UserName == User.Identity.Name);

                var address = new UserAddress
                {
                    FullName = orderDto.ShippingAddress.FullName,
                    Address1 = orderDto.ShippingAddress.Address1,
                    Address2 = orderDto.ShippingAddress.Address2,
                    City = orderDto.ShippingAddress.City,
                    State = orderDto.ShippingAddress.State,
                    Zip = orderDto.ShippingAddress.Zip,
                    Country = orderDto.ShippingAddress.Country,
                };
                user.Address = address;
                _context.Update(user);
            }

            // Ruaj ndryshimet në bazën e të dhënave
            var result = await _context.SaveChangesAsync() > 0;

            // Kontrollo nëse porosia u krijuar me sukses dhe kthe përgjigjen e krijuar
            if (result)
            {
                return CreatedAtRoute("GetOrder", new { id = order.Id }, order.Id);
            }

            // Nëse ndodh një problem gjatë krijimit të porosisë, kthe një përgjigje e gabimit
            return BadRequest("Problem creating order");
        }

    }
}
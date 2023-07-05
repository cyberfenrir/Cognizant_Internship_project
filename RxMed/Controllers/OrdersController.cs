using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using RxMed.Data;
using RxMed.Models;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RxMed.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {

        ApiDbContext _dbContext = new ApiDbContext();


        //Get customer orders by Id

        [HttpGet]
        [Route("[action]")]

        public IActionResult GetCustOrderById(string _id)
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = _dbContext.Users.FirstOrDefault(u => u.email == userEmail);
            var id = int.Parse(_id);
            if (user == null)
            {

                return NotFound();

            }

            else {

                if (user.role_id == 2)
                {

                    var orders = _dbContext.Orders.Where(x => x.user_id == id);
                    if (orders != null)
                    {

                        return Ok(orders);

                    }
                    else {
                    
                        return BadRequest();

                    }
                }
                else {
                    
                    return BadRequest();
                
                }

            }

        }


        //placing order.


        [HttpPost]
        [Route("[action]")]
        public IActionResult PlaceOrder()
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var _user = _dbContext.Users.FirstOrDefault(u => u.email == userEmail);
            if (_user == null)
            {
                return NotFound();
            }

            Order order = new Order();
            order.user_id = _user.user_id;
            var address = _dbContext.Addresses.FirstOrDefault(x => x.user_id == _user.user_id);
            order.address_id = address.address_id;            
            order.order_date = DateTime.Now;
            order.shipping_date = DateTime.Now.AddHours(1);

            _dbContext.Orders.Add(order);
            _dbContext.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPost("add-order-items")]
        public IActionResult AddOrderItems([FromBody] JObject data)
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var _user = _dbContext.Users.FirstOrDefault(u => u.email == userEmail);


            var orderItems = data["orderItems"].ToObject<List<JObject>>();

            if (orderItems == null || orderItems.Count == 0)
            {
                return BadRequest(new { detail = "No Order Items" });
            }

            // (1) Create order

            var order = new Order
            {
                User = _user,
                PaymentMethod = data["paymentMethod"].ToObject<string>(),
                TaxPrice = data["taxPrice"].ToObject<decimal>(),
                ShippingPrice = data["shippingPrice"].ToObject<decimal>(),
                TotalPrice = data["totalPrice"].ToObject<decimal>()
            };
            _dbContext.Orders.Add(order);
            _dbContext.SaveChanges();

            // (2) Create shipping address

            var shipping = new Address
            {
                order_id = order.order_id,
                address = data["address"]["address"].ToObject<string>(),
                city = data["address"]["city"].ToObject<string>(),
                postal = data["address"]["postal"].ToObject<string>(),
                country = data["address"]["country"].ToObject<string>()
            };
            _dbContext.Addresses.Add(shipping);
            _dbContext.SaveChanges();

            // (3) Create order items and set order to orderItem relationship

            foreach (var itemData in orderItems)
            {
                var product = _dbContext.Medicines.Find(itemData["medicine"].ToObject<int>());

                var item = new OrderDetail
                {
                    Medicine = product,
                    Order = order,
                    med_id = product.med_id,
                    med_qty = itemData["qty"].ToObject<int>(),
                    med_price = itemData["price"].ToObject<int>(),
                    Image = product.image_url
                };
                _dbContext.OrderDetails.Add(item);
                _dbContext.SaveChanges();

                // (4) Update stock

                product.quantity -= item.med_qty;
                _dbContext.Medicines.Update(product);
                _dbContext.SaveChanges();
            }
            return Ok("Order Created.");
        }


        [HttpGet("my-orders")]
        public IActionResult GetMyOrders()
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var _user = _dbContext.Users.FirstOrDefault(u => u.email == userEmail);

            var orders = _dbContext.Orders
                .Where(o => o.User == _user)
                .Include(o => o.OrderDetails)
                    .ThenInclude(i => i.Medicine)
                .ToList();

            return Ok(orders);
        }

        [HttpGet("orders")]
        public IActionResult GetOrders()
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var _user = _dbContext.Users.FirstOrDefault(u => u.email == userEmail);


            if (_user.role_id == 1) {
                var orders = _dbContext.Orders
                .Include(o => o.OrderDetails)
                    .ThenInclude(i => i.Medicine)
                .ToList();

                
                return Ok(orders);
            }
            return NotFound();
            
        }


        [HttpGet("orders/{id}")]

        public IActionResult GetOrderById(int id)
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var _user = _dbContext.Users.FirstOrDefault(u => u.email == userEmail);

            try
            {
                var order = _dbContext.Orders
                    .Include(o => o.OrderDetails)
                        .ThenInclude(i => i.Medicine)
                    .FirstOrDefault(o => o.order_id == id);

                if (order == null)
                {
                    return NotFound(new { detail = "Order does not exist" });
                }

                if ( _user.user_id ==1)
                {
                    return BadRequest(new { detail = "Not authorized to view this order" });
                }

                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(new { detail = ex.Message });
            }
        }

        [HttpPost("orders/{id}/pay")]

        public IActionResult UpdateOrderToPaid(int id)
        {
            var order = _dbContext.Orders.Find(id);

            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var _user = _dbContext.Users.FirstOrDefault(u => u.email == userEmail);

            if (order == null)
            {
                return NotFound(new { detail = "Order does not exist" });
            }

            order.IsPaid = true;
            order.PaidAt = DateTime.Now;
            _dbContext.Orders.Update(order);
            _dbContext.SaveChanges();

            return Ok(new { message = "Order was paid" });
        }

        [HttpPost("orders/{id}/deliver")]

        public IActionResult UpdateOrderToDelivered(int id)
        {
            var order = _dbContext.Orders.Find(id);

            if (order == null)
            {
                return NotFound(new { detail = "Order does not exist" });
            }

            order.IsDelivered = true;
            order.DeliveredAt = DateTime.Now;
            _dbContext.Orders.Update(order);
            _dbContext.SaveChanges();

            return Ok(new { message = "Order was delivered" });
        }



    }
}

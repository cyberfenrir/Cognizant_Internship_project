using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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
        [Authorize]
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
        [Authorize]
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


        //[HttpPut]
        //[Authorize]
        //[Route("[action]")]

        ////public IActionResult UpdateOrderAdminSide() { }

    }
}

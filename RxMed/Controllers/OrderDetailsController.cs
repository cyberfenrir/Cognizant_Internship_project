using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RxMed.Data;
using RxMed.Models;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RxMed.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailsController : ControllerBase
    {
        ApiDbContext _dbContext = new ApiDbContext();


        //get all details for admin

        [HttpGet]
        [Authorize]
        [Route("[action]")]
        public IActionResult GetAllOrderDetails()
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = _dbContext.Users.FirstOrDefault(u => u.email == userEmail);

            if (user == null)
            {
                return NotFound();
            }
            else 
            {
                if (user.role_id == 1)
                {
                    return Ok(_dbContext.OrderDetails);
                }
                else {
                    return BadRequest();
                }
            }
        }

        //get all details for user
        [HttpGet]
        [Authorize]
        [Route("[action]")]
        public IActionResult GetByUser()
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = _dbContext.Users.FirstOrDefault(u => u.email == userEmail);

            if (user == null)
            {
                return NotFound();
            }
            else
            {
                if (user.role_id == 2)
                {
                    var order = _dbContext.Orders.FirstOrDefault(x => x.user_id == user.user_id);
                    var orderDetails = _dbContext.OrderDetails.Where(x => x.order_id == order.order_id);
                    return Ok(orderDetails);
                }
                else
                {
                    return BadRequest();
                }
            }
        }


        //post order details

        [HttpPost]
        [Authorize]
        public IActionResult Post([FromBody] OrderDetail orderDetail)
        {
            if (orderDetail == null)
                return NoContent();

            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = _dbContext.Users.FirstOrDefault(u => u.email == userEmail);

            if (user == null) { return  NotFound(); }

            var order = _dbContext.Orders.OrderByDescending(o=>o.order_id).LastOrDefault(x => x.user_id == user.user_id);

            if (order == null)
            {
                return NotFound();
            }
            else {
                if (order != null)
                {

                    orderDetail.order_id = order.order_id;
                    var orderedMedicine = _dbContext.Medicines.FirstOrDefault(m => orderDetail.med_id == m.med_id);
                    if(orderedMedicine == null) { return NotFound(); }
                    orderDetail.med_price = orderedMedicine.price;
                    orderDetail.subtotal = orderDetail.med_price * orderDetail.med_qty;
                    _dbContext.OrderDetails.Add(orderDetail);
                    _dbContext.SaveChanges();
                    return StatusCode(StatusCodes.Status201Created);
                }
                else 
                {
                    
                    return NoContent();
                
                }
                
        
            }
    
        }
    }

}

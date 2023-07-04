using Microsoft.AspNetCore.Mvc;
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

    public class ReviewsController : ControllerBase
    {

        ApiDbContext _dbContext = new ApiDbContext();

        [HttpPost("products/{id}/reviews")]
        public IActionResult CreateProductReview(int id, [FromBody] JObject data)
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = _dbContext.Users.FirstOrDefault(u => u.email == userEmail);
            var product = _dbContext.Medicines.Find(id);

            if (product == null)
            {
                return NotFound(new { detail = "Product does not exist" });
            }

            var alreadyExists = _dbContext.Reviews.Any(r => r.med_id == id && r.User == user);

            if (alreadyExists)
            {
                return BadRequest(new { detail = "Product already reviewed" });
            }

            var rating = data.Value<int>("rating");

            if (rating == 0)
            {
                return BadRequest(new { detail = "Please select a rating" });
            }

            var review = new Review
            {
                User = user,
                med_id = id,
                user_id = user.user_id,
                rating = rating,
                description = data.Value<string>("comment"),
            };

            _dbContext.Reviews.Add(review);
            _dbContext.SaveChanges();

            var reviews = _dbContext.Reviews.Where(r => r.med_id == id).ToList();
            product.total_reviews = reviews.Count;
            _dbContext.Medicines.Update(product);
            _dbContext.SaveChanges();

            return Ok(new { message = "Review Added" });
        }
    }
}
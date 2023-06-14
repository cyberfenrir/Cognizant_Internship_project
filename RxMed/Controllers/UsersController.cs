using Microsoft.AspNetCore.Mvc;
using RxMed.Data;
using RxMed.Models;

namespace RxMed.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        ApiDbContext _dbContext = new ApiDbContext();


        // GET: api/<UsersController>
        [HttpGet]
        public IEnumerable<User> Get()
        {
            return _dbContext.Users;
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public User Get(int id)
        {
            var temp = _dbContext.Users.FirstOrDefault(x => x.user_id == id);

            if (temp != null)
            {

                return temp;

            }
            else {

                throw new Exception();

            }
        }

        // POST api/<UsersController>
        [HttpPost]
        public IActionResult Post([FromBody] User user)
        {

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            return Ok("Customer Created Successfully");


        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol.Plugins;
using RxMed.Data;
using RxMed.DTO;
using RxMed.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RxMed.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        ApiDbContext _dbContext = new ApiDbContext();

        private IConfiguration _config;
        public UsersController(IConfiguration config)
        {
            _config = config;
        }

        // GET: api/<UsersController>
        [HttpGet("GetAllUserData")]
        [Authorize]
        //[Authorize(Roles = "Admin")]
        public IActionResult GetAllUserData()
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = _dbContext.Users.FirstOrDefault(u => u.email == userEmail);
            if (user.role_id == 1)
            {

                var UserDetails = _dbContext.Users.Select(
                    t => new UserDTO
                    {
                        User_id = t.user_id,
                        Username = t.username,
                        Email = t.email,
                        First_name = t.first_name,
                        Last_name = t.last_name,
                        Password = t.password,
                        RoleName = t.Role.role_name,
                        UserAddress = t.Addresses.First().address + ", " + t.Addresses.First().city + ", " + t.Addresses.First().state + ", " + t.Addresses.First().postal + ", " + t.Addresses.First().country


                    }
                );
                if (UserDetails == null)
                {

                    return BadRequest();

                }
                else
                {

                    return Ok(UserDetails);

                }
            }
            else {
            
                return BadRequest();
            
            }
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

        //Register

        [HttpPost("[action]")]
        //https://localhost:7197/api/users/register
        public IActionResult Register([FromBody] User user)
        {
            var userExists = _dbContext.Users.FirstOrDefault(u => u.email == user.email);

            if (userExists != null)
            {
                return BadRequest("User with same email id already exists");
            }
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }





        //Login


        [HttpPost("[action]")]
        //https://localhost:7197/api/users/login
        public IActionResult Login([FromBody] User user)
        {
            var currentUser = _dbContext.Users.FirstOrDefault(x => x.email == user.email && x.password == user.password);

            if (currentUser == null) { return NotFound(); }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] { new Claim(ClaimTypes.Email, user.email) };

            var token = new JwtSecurityToken(
                issuer: _config["JWT:Issuer"],
                audience: _config["JWT:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(jwt);
        }

        

    }
}

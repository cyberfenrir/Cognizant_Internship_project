using Azure.Identity;
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

        // Get
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

        //getbyId


        //[HttpGet("/api/Users/GetUserDetailsById/{id}")]
        //[Authorize]

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetUserDetailsById(string _Id)
        {
            //var user = await _dbContext.Users.FindAsync(Id);
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = _dbContext.Users.FirstOrDefault(u => u.email == userEmail);
            int Id = int.Parse(_Id);

            if (user != null)
            {
                
                    var ViewUser = _dbContext.Users.Where(x=>x.user_id == Id).Select(
                        v => new UserDTO()
                        {

                            User_id = Id,
                            Username = v.username,
                            Email = v.email,

                            First_name = v.first_name,
                            Last_name = v.last_name,
                            Password = v.password,
                            RoleName = v.Role.role_name,
                            UserAddress = v.Addresses.First().address + ", " + v.Addresses.First().city + ", " + v.Addresses.First().state + ", " + v.Addresses.First().postal + ", " + v.Addresses.First().country

                        }

                    );
                    return Ok(ViewUser);
                
                
            }
            else
            {
                return NotFound("User not found");
               
            }

        }


        [HttpPut]
        [Authorize]
        [Route("[action]")]
        public async Task<IActionResult> UpdateUserProfile([FromBody] User user)
        {

            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var _user = _dbContext.Users.FirstOrDefault(u => u.email == userEmail);

            
                _user.first_name = user.first_name;
                _user.last_name = user.last_name;
                _user.username = user.username;
                _user.email = user.email;
                _user.password = user.password;

                await _dbContext.SaveChangesAsync();
                return Ok("Record updated successfully");
           
        }





        //Delete


        [HttpDelete]
        [Authorize]
        [Route("[action]")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = _dbContext.Users.FirstOrDefault(u => u.email == userEmail);

            if (user.role_id == 1)
            {
                var userdetails = await _dbContext.Users.FindAsync(id);
                if (userdetails == null)
                {
                    return NotFound("User Id not Find");
                }
                _dbContext.Remove(userdetails);
                await _dbContext.SaveChangesAsync();
                return Ok("User successfully deleted");
            }
            else { 
                return BadRequest();
            }
        }


        //Update

        //[HttpPut]
        //[Authorize]
        //[Route("[action]")]
        //public async Task<IActionResult> UpdateUser(string _uid,[FromBody] User user)
        //{

        //    var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        //    var _user = _dbContext.Users.FirstOrDefault(u => u.email == userEmail);

        //    int uid = int.Parse(_uid);
        //    var userExists = await _dbContext.Users.FindAsync(uid);
        //    if (userExists != null)
        //    {
        //        userExists.first_name = user.first_name;
        //        userExists.last_name = user.last_name;
        //        userExists.username = user.username;
        //        userExists.email = user.email;
        //        userExists.password = user.password;

        //        await _dbContext.SaveChangesAsync();
        //        return Ok("Record updated successfully");
        //    }
        //    else { return NotFound("User with that Id is not found"); }
        //}



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
        public IActionResult Login( User user)
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


        //logout

        [HttpPost("[action]")]
        //https://localhost:7197/api/users/logout
        public async Task<IActionResult> Logout()
        {
            // Invalidate the token by setting its expiration time to a past date/time
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var jwtHandler = new JwtSecurityTokenHandler();
            var jwtToken = jwtHandler.ReadToken(token) as JwtSecurityToken;

            var expiredToken = new JwtSecurityToken(
                _config["JWT:Issuer"],
                _config["JWT:Audience"],
                jwtToken.Claims,
                DateTime.Now,
                DateTime.Now.AddMinutes(-60),  // Expired token with negative expiration time
                jwtToken.SigningCredentials
            );
            var newToken = jwtHandler.WriteToken(expiredToken);

            return Ok("Logout successful");
        }





    }
}

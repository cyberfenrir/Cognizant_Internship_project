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
    public class MedicinesController : ControllerBase
    {

        ApiDbContext _dbContext = new ApiDbContext();

        private IConfiguration _config;
        public MedicinesController(IConfiguration config)
        {
            _config = config;
        }

        //GetMedData

        [HttpGet("GetAllMedData")]
        
        public IActionResult GetAllMedData()
        {
            
           

                var MedDetails = _dbContext.Medicines.Select(
                    t => new MedicineDTO
                    {
                        med_id = t.med_id,
                        med_name = t.med_name,
                        med_pharma = t.med_pharma,
                        description = t.description,
                        rx = t.rx,
                        price = t.price,
                        total_reviews = t.total_reviews,
                        avg_reviews = t.avg_reviews,
                        out_of_stock = t.out_of_stock,
                        quantity = t.quantity,
                        image_url = t.image_url

                    }
                );
                if (MedDetails == null)
                {

                    return BadRequest();

                }
                else
                {

                    return Ok(MedDetails);

                }
         
            
        }


        //getbyId

        [HttpGet]
      
        [Route("[action]")]
        public async Task<IActionResult> GetMedDetailsById(string _Id)
        {
            
            int Id = int.Parse(_Id);


            var t = _dbContext.Medicines.FirstOrDefault(p => p.med_id == Id);

            if (t == null)
            {
                return NotFound();
            }

            var productDto = new MedicineDTO
            {
                med_id = t.med_id,
                med_name = t.med_name,
                med_pharma = t.med_pharma,
                description = t.description,
                rx = t.rx,
                price = t.price,
                total_reviews = t.total_reviews,
                avg_reviews = t.avg_reviews,
                out_of_stock = t.out_of_stock,
                quantity = t.quantity,
                image_url = t.image_url
            };

            return Ok(productDto);



        }





        //Delete


        [HttpDelete]
        [Authorize]
        [Route("[action]")]
        public async Task<IActionResult> DeleteMed(int id)
        {

            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = _dbContext.Users.FirstOrDefault(u => u.email == userEmail);

            if (user.role_id == 1)
            {
                var medDetails = await _dbContext.Medicines.FindAsync(id);
                if (medDetails == null)
                {
                    return NotFound("User Id not Find");
                }
                _dbContext.Remove(medDetails);
                await _dbContext.SaveChangesAsync();
                return Ok("User successfully deleted");
            }
            else
            {
                return BadRequest();
            }
        }


        //Update

        [HttpPut]
        [Authorize]
        [Route("[action]")]
        public async Task<IActionResult> UpdateMed(string _uid, [FromBody] Medicine med)
        {



            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var _user = _dbContext.Users.FirstOrDefault(u => u.email == userEmail);
            int uid = int.Parse(_uid);

            if (_user.role_id == 1)
            {

                var medExists = await _dbContext.Medicines.FindAsync(uid);
                if (medExists != null)
                {
                    medExists.med_name = med.med_name;
                    medExists.med_pharma = med.med_pharma;
                    medExists.price = med.price;
                    medExists.quantity = med.quantity;
                    medExists.description = med.description;
                    medExists.image_url = med.image_url;
                    medExists.rx = medExists.rx;

                    await _dbContext.SaveChangesAsync();
                    return Ok("Record updated successfully");
                }
                else
                {
                    return NotFound("User with that Id is not found");
                }

            }
            else {

                return BadRequest();

            }
        }

        //Add

        [HttpPost]
        [Authorize]
        [Route("[action]")]
        public IActionResult AddMedicine([FromBody] Medicine med)
        {

            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var _user = _dbContext.Users.FirstOrDefault(u => u.email == userEmail);

            if (_user.role_id == 1) {
            var medExists = _dbContext.Medicines.FirstOrDefault(u => u.med_name == med.med_name);

            if (medExists != null)
            {
                return BadRequest("Medicine with same name already exists.");
            }
            _dbContext.Medicines.Add(med);
            _dbContext.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);

            }
            else
            {

                return BadRequest();

            }
        }






    }
}

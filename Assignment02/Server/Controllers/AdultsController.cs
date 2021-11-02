using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Model;
using Server.Persistence.AdultPersistence;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdultsController : ControllerBase
    {
        private readonly IAdultPersistence adultPersistence;

        public AdultsController(IAdultPersistence adultPersistence)
        {
            this.adultPersistence = adultPersistence;
        }

        [HttpGet]
        public async Task<ActionResult<IList<Adult>>> GetAllAdultsAsync([FromQuery] string firstName, [FromQuery] string jobTitle)
        {
            var adultsToReturn = new List<Adult>();
            try
            {
                var adults = await adultPersistence.GetAllAdultsAsync();
                adultsToReturn = adults.Where(a =>
                    (string.IsNullOrEmpty(firstName) || a.FirstName.ToLower().Contains(firstName.ToLower())) &&
                    (string.IsNullOrEmpty(jobTitle) || a.JobTitle.JobTitle.ToLower().Contains(jobTitle.ToLower()))).ToList();
                
                return Ok(adultsToReturn);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Adult>> GetAdultByIdAsync(int id)
        {
            try
            {
                var adultToReturn = await adultPersistence.GetAdultByIdAsync(id);
                return Ok(adultToReturn);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        
        [HttpPost]
        public async Task<ActionResult<Adult>> AddAdultAsync([FromBody] Adult adult)
        {
            try
            {
                var adultToReturn = await adultPersistence.AddAdultAsync(adult);
                return Created($"/{adultToReturn.Id}", adult);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult> DeleteAdultAsync([FromRoute] int id)
        {
            try
            {
                await adultPersistence.RemoveAdultAsync(id);
                Console.WriteLine(GetAllAdultsAsync(null, null));
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }
        
        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult<Adult>> EditAdultAsync([FromBody] Adult adult)
        {
            try
            {
                var adultToReturn = await adultPersistence.EditAdultAsync(adult);
                return Ok(adultToReturn);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
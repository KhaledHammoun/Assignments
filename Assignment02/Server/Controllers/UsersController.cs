using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Model;
using Server.Persistence.UserPersistence;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserPersistence userPersistence;

        public UsersController(IUserPersistence userPersistence)
        {
            this.userPersistence = userPersistence;
        }

        [HttpGet]
        public async Task<ActionResult<IList<User>>> GetAllUsers()
        {
            try
            {
                var allUsersAsync = await userPersistence.GetAllUsersAsync();
                return Ok(allUsersAsync);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<User>> CreateUser([FromBody] User user)
        {
            try
            {
                var userToReturn = await userPersistence.CreateUserAsync(user);
                return Created($"/{userToReturn.Username}",userToReturn);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        [Route("validate")]
        public async Task<ActionResult<User>> ValidateUser([FromBody] User user)
        {
            try
            {
                var validateUserAsync = await userPersistence.ValidateUserAsync(user);
                return Ok(validateUserAsync);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using SandTigerShark.GameServer.Services.Commands;
using SandTigerShark.GameServer.Services.Users;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SandTigerShark.Controllers
{
    [Route("api/users")]
    public class UsersController : Controller
    {
        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        /// <summary>
        /// Create a user based on his user name.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.MethodNotAllowed)]
        public async Task<IActionResult> CreateUser([FromBody]CreateUser command)
        {
            var userToken = await userService.CreateUser(command);
            return Created($"api/users/{userToken}", userToken);
        }

        /// <summary>
        /// Get a user based on it user token.
        /// </summary>
        /// <param name="userToken"></param>
        /// <returns></returns>
        [HttpGet("{userToken}")]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetUser(Guid userToken)
        {
            var user = await userService.GetUser(userToken);
            return Ok(user);
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using SandTigerShark.GameServer.Services.Commands;
using SandTigerShark.GameServer.Services.Users;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SandTigerShark.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.MethodNotAllowed)]
        public async Task<IActionResult> GetToken([FromBody]CreateUser command)
        {
            var userToken = await userService.CreateUser(command);
            return CreatedAtAction("GetToken", userToken);
        }

        [HttpGet("{userToken}")]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.MethodNotAllowed)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetUser(Guid userToken)
        {
            var user = await userService.GetUser(userToken);
            return Ok(user);
        }
    }
}
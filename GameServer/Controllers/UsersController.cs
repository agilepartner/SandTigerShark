using Microsoft.AspNetCore.Mvc;
using SandTigerShark.GameServer.Repositories;
using System;
using System.Threading.Tasks;

namespace SandTigerShark.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IUserRepository userRepository;

        public UsersController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        [HttpGet("token/{userName}")]
        public async Task<IActionResult> GetToken(string userName)
        {
            Guid token = await this.userRepository.GetUserToken(userName);
            return Ok(token);
        }

     
    }
}

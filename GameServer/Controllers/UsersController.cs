using Microsoft.AspNetCore.Mvc;
using SandTigerShark.Repositories;

namespace SandTigerShark.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {

        [HttpGet("token/{userName}")]
        public string GetToken(string userName)
        {
            return new UserRepository().GetUserToken(userName).ToString();
        }

     
    }
}

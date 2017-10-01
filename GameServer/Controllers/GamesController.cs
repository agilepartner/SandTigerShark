using Microsoft.AspNetCore.Mvc;
using System;

namespace SandTigerShark.Controllers
{
    [Route("api/[controller]")]
    public class GamesController : Controller
    {

        [HttpGet("new/{userToken}")]
        public string GetNewGameId(string userToken)
        {
            throw new NotImplementedException();
        }

        [HttpGet("gameState/{gameId}")]
        public string GetGameState(string gameId)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public void Post([FromBody]string command)
        {
            throw new NotImplementedException();
        }


    }
}

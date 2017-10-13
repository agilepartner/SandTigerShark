using GameServer.Repositories;
using Microsoft.AspNetCore.Mvc;
using SandTigerShark.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace SandTigerShark.Controllers
{
    [Route("api/[controller]")]
    public class GamesController : Controller
    {

        private GameRepository gameRepository = new GameRepository();

        [HttpGet("new/{userToken}")]
        public string GetNewGameId(string userToken)
        {
            return gameRepository.GetOrCreateNewGame(userToken);
        }

        [HttpGet("gameState/{gameId}")]
        public async Task<IActionResult> GetGameState(string gameId)
        {
            GameStatus gameStatus = await gameRepository.GetGameStatus(gameId);
            return Ok(gameStatus);
        }

        [HttpPost]
        public void Post([FromBody]string command)
        {
            throw new NotImplementedException();
        }


    }
}

using GameServer.Services.Repositories;
using Microsoft.AspNetCore.Mvc;
using SandTigerShark.Services.Models;
using System;
using System.Threading.Tasks;

namespace SandTigerShark.Controllers
{
    [Route("api/[controller]")]
    public class GamesController : Controller
    {
        private readonly IGameRepository gameRepository;

        public GamesController(IGameRepository gameRepository)
        {
            this.gameRepository = gameRepository;
        }

        [HttpGet("{userToken}")]
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
        public void Post([FromBody]string instruction)
        {
            throw new NotImplementedException();
        }
    }
}
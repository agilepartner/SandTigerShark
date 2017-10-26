using GameServer.Services.Repositories;
using Microsoft.AspNetCore.Mvc;
using SandTigerShark.GameServer.Utils;
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

        [HttpGet("available")]
        public IActionResult GetAvailableGame()
        {
            Guid availableGameId = this.gameRepository.GetAvailableGame();
            if (Guid.Empty.Equals(availableGameId))
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpPost("")]
        public IActionResult Post()
        {
            this.gameRepository.CreateGame();
            return Ok();
        }


        [HttpGet("gameState/{gameId}")]
        public async Task<IActionResult> GetGameState(string gameId)
        {
            return await HttpContext.Call(async (userToken) =>
            {
                var gameStatus = await gameRepository.GetGameStatus(gameId, userToken);
                return Ok(gameStatus);
            });
        }

        
    }
}
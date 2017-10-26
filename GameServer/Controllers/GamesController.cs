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
        public async Task<IActionResult> GetAvailableGame()
        {
            return await HttpContext.Call(async (userToken) =>
            {
                Guid availableGameId = await gameRepository.GetAvailableGame();
                if (Guid.Empty.Equals(availableGameId))
                {
                    return NotFound();
                }
                return Ok(availableGameId);
            });
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateGame()
        {
            return await HttpContext.Call(async (userToken) =>
            {
                await gameRepository.CreateGame();
                return Ok();
            });
        }


        [HttpGet("gameState/{gameId}")]
        public async Task<IActionResult> GetGameState(Guid gameId)
        {
            return await HttpContext.Call(async(userToken) =>
            {
                var gameStatus = await gameRepository.GetGameStatus(gameId, userToken);
                return Ok(gameStatus);
            });
        }
    }
}
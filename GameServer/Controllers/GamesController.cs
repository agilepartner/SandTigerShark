using GameServer.Services.Repositories;
using Microsoft.AspNetCore.Mvc;
using SandTigerShark.GameServer.Utils;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SandTigerShark.Controllers
{
    [Route("api/[controller]")]
    public class GamesController : Controller
    {
        private readonly IGameRepository gameRepository;

        /// <summary>
        /// Manage your games through this api
        /// </summary>
        /// <param name="gameRepository"></param>
        public GamesController(IGameRepository gameRepository)
        {
            this.gameRepository = gameRepository;
        }

        [HttpGet("available")]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> GetAvailableGame()
        {
            Guid availableGameId = await gameRepository.GetAvailableGame();
            if (Guid.Empty.Equals(availableGameId))
            {
                return NotFound();
            }
            return Ok(availableGameId);
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateGame()
        {
            await gameRepository.CreateGame();
            return Ok();
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
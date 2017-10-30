using Microsoft.AspNetCore.Mvc;
using SandTigerShark.GameServer.Controllers;
using SandTigerShark.GameServer.Services.Commands;
using SandTigerShark.GameServer.Services.Dtos;
using SandTigerShark.GameServer.Services.Games;
using SandTigerShark.GameServer.Services.Users;
using SandTigerShark.GameServer.Utils;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SandTigerShark.Controllers
{
    [Route("api/[controller]")]
    public class GamesController : Controller
    {
        private readonly IGameService gameService;
        private readonly IUserRepository userRepository;

        /// <summary>
        /// Manage your games through this api
        /// </summary>
        /// <param name="gameService"></param>
        public GamesController(
            IGameService gameService,
            IUserRepository userRepository)
        {
            this.gameService = gameService;
            this.userRepository = userRepository;
        }

        [HttpGet("available")]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAvailableGame()
        {
            return await HttpContext.Invoke(userRepository, async (userToken) =>
            {
                var availableGameId = await gameService.GetAvailableGame(userToken);
                return Ok(availableGameId);
            });
        }

        [HttpPost]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.MethodNotAllowed)]
        public async Task<IActionResult> CreateGame([FromBody]CreateGame command)
        {
            return await HttpContext.Invoke(userRepository, async (userToken) =>
            {
                var gameId = await gameService.CreateGame(command, userToken);
                return CreatedAtAction("GetGameState", new { id = gameId });
            });
        }

        [HttpGet("{gameId}")]
        [ProducesResponseType(typeof(GameStatus), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetGameState(Guid gameId)
        {
            return await HttpContext.Invoke(userRepository, async (userToken) =>
            {
                var gameStatus = await gameService.GetGameStatus(gameId, userToken);
                return Ok(gameStatus);
            });
        }

        [HttpPut]
        [ProducesResponseType(typeof(Game), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.MethodNotAllowed)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Play([FromBody]Play command)
        {
            return await HttpContext.Invoke(userRepository, async (userToken) =>
            {
                await gameService.Play(command, userToken);
                return Ok();
            });
        }
    }
}
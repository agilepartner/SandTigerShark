using Microsoft.AspNetCore.Mvc;
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
    [Route("api/games")]
    public class GamesController : Controller
    {
        private readonly IGameService gameService;
        private readonly IUserRepository userRepository;

        public GamesController(
            IGameService gameService,
            IUserRepository userRepository)
        {
            this.gameService = gameService;
            this.userRepository = userRepository;
        }

        /// <summary>
        /// Return an available game for the given GameType.
        /// </summary>
        /// <returns></returns>
        [HttpGet("available/{gameType}")]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAvailableGame(GameType gameType)
        {
            return await HttpContext.Invoke(userRepository, async (userToken) =>
            {
                var availableGameId = await gameService.GetAvailableGame(gameType, userToken);
                return Ok(availableGameId);
            });
        }

        /// <summary>
        /// Create a new game with you as 1st player.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.MethodNotAllowed)]
        public async Task<IActionResult> CreateGame([FromBody]CreateGame command)
        {
            return await HttpContext.Invoke(userRepository, async (userToken) =>
            {
                var gameId = await gameService.CreateGame(command, userToken);
                return Created($"api/games/{gameId}", gameId);
            });
        }

        /// <summary>
        /// Returns the state of a given game.
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        [HttpGet("{gameId}")]
        [ProducesResponseType(typeof(GameStatus), (int)HttpStatusCode.OK)]
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

        /// <summary>
        /// Play a command on a given game.
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("{gameId}")]
        [ProducesResponseType(typeof(Game), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.MethodNotAllowed)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Play(Guid gameId, [FromBody]Play command)
        {
            return await HttpContext.Invoke(userRepository, async (userToken) =>
            {
                await gameService.Play(gameId, command, userToken);
                return Ok();
            });
        }
    }
}
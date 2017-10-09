using GameServer.Repositories;
using Microsoft.AspNetCore.Mvc;
using SandTigerShark.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;

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
        public HttpResponseMessage GetGameState(string gameId)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.NotFound);
            GameStatus gameStatus = gameRepository.GetGameStatus(gameId);
            if (gameStatus != null)
            {
                response.Content = new ObjectContent<GameStatus>(gameStatus, new JsonMediaTypeFormatter(), "application/jsons");
                response.StatusCode = HttpStatusCode.OK;
               
            }
            return response;
        }

        [HttpPost]
        public void Post([FromBody]string command)
        {
            throw new NotImplementedException();
        }


    }
}

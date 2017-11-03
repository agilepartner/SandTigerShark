using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using SandTigerShark.TicTacToe.App.Commands;
using SandTigerShark.TicTacToe.App.Dtos;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SandTigerShark.TicTacToe.App
{
    internal class TicTacToeRepository
    {
        private readonly IRestProxy restProxy;
        private readonly string baseUrl;

        public TicTacToeRepository(
                IOptions<GameConfig> configuration,
                IRestProxy proxy)
        {
            restProxy = proxy;
            baseUrl = $"{configuration.Value.Url}games/";
        }

        internal async Task<Guid> CreateOrGetAvailableGame()
        {
            Guid? availableGame = null;

            availableGame = await restProxy.GetAsync(
                $"{baseUrl}available/TicTacToe",
                (response) =>
                {
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        return CreateGame();
                    }
                    throw new HttpRequestException($"{response.ReasonPhrase} : {response.RequestMessage.RequestUri.AbsoluteUri}");
                });

            return availableGame.Value;
        }

        internal async Task<GameStatus> GetGameState(Guid gameId)
        {
            return await restProxy.GetAsync<GameStatus>($"{baseUrl}{gameId}");
        }

        internal async Task<PlayResult> Play(
            Guid gameId,
            int position)
        {
            var result = new PlayResult { Success = true };
            var command = new Play { Instruction = position };

            try
            {
                await restProxy.PutAsync($"{baseUrl}{gameId}", command);
                var status = await GetGameState(gameId);
                result.Board = ((JArray)status.LastState).Values<int>().ToArray();
            }
            catch (HttpRequestException httpRequestException)
            {
                result.Message = httpRequestException.Message;
                result.Success = false;
            }
            return result;
        }

        private async Task<Guid> CreateGame()
        {
            var command = new CreateGame
            {
                Type = "TicTacToe"
            };
            return await restProxy.PostAsync<CreateGame, Guid>($"{baseUrl}", command);
        }
    }
}
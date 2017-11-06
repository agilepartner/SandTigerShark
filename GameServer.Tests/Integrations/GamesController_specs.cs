using FluentAssertions;
using Newtonsoft.Json.Linq;
using SandTigerShark.GameServer.Services.Commands;
using SandTigerShark.GameServer.Services.Dtos;
using SandTigerShark.GameServer.Services.Games;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace SandTigerShark.GameServer.Tests.Integrations
{
    public class GamesController_specs
    {
        private const string baseUrl = "api/games";

        public class get_available_game_should : WebApiTest<Startup>
        {
            public class return_OK : get_available_game_should
            {
                [Fact]
                public async Task when_a_game_is_available()
                {
                    var command = new CreateGame { Type = Services.Games.GameType.TicTacToe };
                    var authorizedClient = await GetClientWithUserToken();

                    using (var createResponse = await authorizedClient.PostAsync($"{baseUrl}", command))
                    {
                        var availableGame = await createResponse.Content.ReadAsync<Guid>();

                        using (var availableGameResponse = await authorizedClient.GetAsync($"{baseUrl}/available/TicTacToe"))
                        {
                            availableGameResponse.StatusCode.Should().Be(HttpStatusCode.OK);

                            var gameId = await availableGameResponse.Content.ReadAsync<Guid>();
                            gameId.Should().Be(availableGame);
                        }
                    }
                }
            }

            public class return_not_found : get_available_game_should
            {
                [Fact]
                public async Task when_no_game_has_been_created()
                {
                    var client = await GetClientWithUserToken();
                    using (var response = await client.GetAsync($"{baseUrl}/available/TicTacToe"))
                    {
                        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
                    }
                }
            }

            public class return_unauthorized : get_available_game_should
            {
                [Fact]
                public async Task when_user_is_not_recognized()
                {
                    using (var response = await Client.GetAsync($"{baseUrl}/available/TicTacToe"))
                    {
                        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
                    }
                }
            }
        }

        public class post_create_game_should : WebApiTest<Startup>
        {
            public class return_created : post_create_game_should
            {
                [Fact]
                public async Task when_game_type_is_supported()
                {
                    var command = new CreateGame { Type = Services.Games.GameType.TicTacToe };
                    var authorizedClient = await GetClientWithUserToken();

                    using (var response = await authorizedClient.PostAsync($"{baseUrl}", command))
                    {
                        response.StatusCode.Should().Be(HttpStatusCode.Created);

                        var result = await response.Content.ReadAsync<Guid?>();
                        result.Should().NotBeNull();
                        result.Should().NotBeEmpty();
                        response.Headers.Location.OriginalString.Should().Be($"api/games/{result}");
                    }
                }
            }

            public class return_unauthorized : post_create_game_should
            {
                [Fact]
                public async Task when_user_is_not_recognized()
                {
                    var command = new CreateGame();

                    using (var response = await Client.PostAsync($"{baseUrl}/", command))
                    {
                        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
                    }
                }
            }
        }

        public class get_game_status_should : WebApiTest<Startup>
        {
            public class return_OK : get_game_status_should
            {
                [Fact]
                public async Task when_the_game_exists()
                {
                    var command = new CreateGame { Type = Services.Games.GameType.TicTacToe };
                    var authorizedClient = await GetClientWithUserToken();

                    using (var createResponse = await authorizedClient.PostAsync($"{baseUrl}", command))
                    {
                        var availableGame = await createResponse.Content.ReadAsync<Guid>();

                        using (var availableGameResponse = await authorizedClient.GetAsync($"{baseUrl}/{availableGame}"))
                        {
                            availableGameResponse.StatusCode.Should().Be(HttpStatusCode.OK);

                            var gameStatus = await availableGameResponse.Content.ReadAsync<GameStatus>();
                            gameStatus.Should().NotBeNull();
                            gameStatus.LastState.Should().BeOfType<JArray>();
                            gameStatus.Status.Should().Be(Status.Created);
                        }
                    }
                }
            }

            public class return_not_found : get_game_status_should
            {
                [Fact]
                public async Task when_no_game_has_been_created()
                {
                    var client = await GetClientWithUserToken();
                    using (var response = await client.GetAsync($"{baseUrl}/{Guid.NewGuid()}"))
                    {
                        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
                    }
                }
            }

            public class return_unauthorized : get_game_status_should
            {
                [Fact]
                public async Task when_user_is_not_recognized()
                {
                    using (var response = await Client.GetAsync($"{baseUrl}/{Guid.NewGuid()}"))
                    {
                        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
                    }
                }
            }
        }

        public class put_play_should : WebApiTest<Startup>
        {
            public class return_OK : put_play_should
            {
                // Integration test depending on the Azure function 
                public async Task when_game_started_and_instruction_valid()
                {
                    var command = new CreateGame { Type = GameType.TicTacToe };
                    var authorizedClientForPlayer1 = await GetClientWithUserToken();

                    using (var createGameResponse = await authorizedClientForPlayer1.PostAsync($"{baseUrl}", command))
                    {
                        var gameId = await createGameResponse.Content.ReadAsync<Guid>();
                        var authorizedClientForPlayer2 = await GetClientWithUserToken("AnotherUser");
                        var play = new Play { Instruction = 8 };

                        using (var playResponse = await authorizedClientForPlayer2.PutAsync($"{baseUrl}/{gameId}", play))
                        {
                            playResponse.StatusCode.Should().Be(HttpStatusCode.OK);
                        }
                    }
                }
            }

            public class return_method_not_allowed : put_play_should
            {
                [Fact]
                public async Task when_instruction_is_null()
                {
                    var authorizedClient = await GetClientWithUserToken();
                    var command = new Play();

                    using (var response = await authorizedClient.PutAsync($"{baseUrl}/{Guid.NewGuid()}", command))
                    {
                        response.StatusCode.Should().Be(HttpStatusCode.MethodNotAllowed);
                    }
                }
            }

            /*public class return_not_found : put_play_should
            {
                [Fact]
                public async Task when_game_id_does_not_exist()
                {
                    var command = new Play();
                    var authorizedClient = await GetClientWithUserToken();

                    using (var response = await authorizedClient.PutAsync($"{baseUrl}/{Guid.NewGuid()}", command))
                    {
                        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
                    }
                }
            }*/

            public class return_unauthorized : put_play_should
            {
                [Fact]
                public async Task when_user_is_not_recognized()
                {
                    var command = new CreateGame();

                    using (var response = await Client.PutAsync($"{baseUrl}/{Guid.NewGuid()}", command))
                    {
                        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
                    }
                }
            }
        }
    }
}
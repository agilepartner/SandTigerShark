using FluentAssertions;
using SandTigerShark.GameServer.Services.Exceptions;
using SandTigerShark.GameServer.Services.Games;
using SandTigerShark.GameServer.Services.Utils;
using SandTigerShark.GameServer.Tests.TicTacToes;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Xunit;


namespace SandTigerShark.GameServer.Tests.Games
{
    public class GameRepository_specs
    {
        private readonly GameRepository gameRepository;

        public GameRepository_specs()
        {
            gameRepository = new GameRepository();
        }

        public class get_available_game_should : GameRepository_specs
        {
            public class raise_a_not_found_exception : GameRepository_specs
            {
                [Fact]
                public async Task when_no_game_found_for_a_given_type()
                {
                    var exception = await Assert.ThrowsAsync<NotFoundException>(() => gameRepository.GetAvailableGame(GameType.TicTacToe));
                    exception.Message.Should().Be("No available game found for type : TicTacToe");
                }
            }

            [Fact]
            public async Task returns_the_first_available_game_for_a_given_type()
            {
                var ticTacToe = TicTacToeApi.Create();
                InitializeGamesInRepository(ticTacToe);

                var availableGame = await gameRepository.GetAvailableGame(GameType.TicTacToe);
                availableGame.Should().Be(ticTacToe.Id);
            }
        }

        public class get_by_id_should : GameRepository_specs
        {
            protected readonly Guid gameId;

            public get_by_id_should()
            {
                gameId = Guid.NewGuid();
            }

            public class raise_a_not_found_exception : get_by_id_should
            {
                [Fact]
                public async Task when_no_game_found_for_a_given_id()
                {
                    var exception = await Assert.ThrowsAsync<NotFoundException>(() => gameRepository.GetById(gameId));
                    exception.Message.Should().Be("No game found with id : " + gameId);
                }
            }

            [Fact]
            public async Task returns_the_game_for_a_given_id()
            {
                var ticTacToe = TicTacToeApi.Create();
                InitializeGamesInRepository(ticTacToe);

                var game = await gameRepository.GetById(ticTacToe.Id);
                game.Should().Be(ticTacToe);
            }
        }

        public class save_should : GameRepository_specs
        {
            protected readonly Game gameToSave;

            public save_should()
            {
                gameToSave = TicTacToeApi.Create();
            }

            [Fact]
            public async Task insert_the_game_when_not_already_in_memory()
            {
                await gameRepository.Save(gameToSave);
                AssertGamesInRepository();
            }

            [Fact]
            public async Task update_the_game_when_already_in_memory()
            {
                InitializeGamesInRepository(gameToSave);

                await gameRepository.Save(gameToSave);

                AssertGamesInRepository();
            }

            private void AssertGamesInRepository()
            {
                var gamesInRepository = Reflect<GameRepository>.GetFieldValue<ConcurrentDictionary<Guid, Game>>(gameRepository, "games");

                gamesInRepository.Should().HaveCount(1);
                gamesInRepository.Single().Value.Should().Be(gameToSave);
            }
        }

        protected void InitializeGamesInRepository(params Game[] games)
        {
            var gameDictionary = games.ToDictionary(g => g.Id, g => g);
            Reflect<GameRepository>.SetFieldValue(gameRepository, "games", new ConcurrentDictionary<Guid, Game>(gameDictionary));
        }
    }
}
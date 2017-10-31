using FakeItEasy;
using FluentAssertions;
using SandTigerShark.GameServer.Services.Commands;
using SandTigerShark.GameServer.Services.Exceptions;
using SandTigerShark.GameServer.Services.Games;
using SandTigerShark.GameServer.Services.Utils;
using SandTigerShark.GameServer.Tests.TicTacToes;
using System;
using System.Threading.Tasks;
using Xunit;


namespace SandTigerShark.GameServer.Tests.Games
{
    public class GameService_specs
    {
        private readonly IGameFactory factory;
        private readonly IGameRepository repository;
        private readonly GameService gameService;
        private readonly Guid userToken;

        public GameService_specs()
        {
            userToken = Guid.NewGuid();
            factory = A.Fake<IGameFactory>();
            repository = A.Fake<IGameRepository>();

            gameService = new GameService(factory, repository);
        }

        public class create_game_should : GameService_specs
        {
            private readonly Game createdGame;

            public create_game_should()
            {
                createdGame = TicTacToeApi.Create();
                A.CallTo(() => factory.Create(A<GameType>.Ignored, userToken)).Returns(createdGame);
            }

            public class raise_invalid_command_exception : create_game_should
            {
                [Fact]
                public async Task when_command_is_null()
                {
                    var exception = await Assert.ThrowsAsync<InvalidCommandException>(() => gameService.CreateGame(null, userToken));
                    exception.Message.Should().Be("CreateGame command is required");
                }
            }

            [Fact]
            public async Task create_and_store_a_new_game()
            {
                var command = new CreateGame { Type = GameType.TicTacToe };

                var newGameId = await gameService.CreateGame(command, userToken);

                A.CallTo(() => factory.Create(command.Type, userToken)).MustHaveHappened(Repeated.Exactly.Once);
                A.CallTo(() => repository.Save(createdGame)).MustHaveHappened(Repeated.Exactly.Once);
                newGameId.Should().Be(createdGame.Id);
            }
        }

        public class get_available_game_should : GameService_specs
        {
            [Fact]
            public async Task returns_the_first_available_game()
            {
                var gameType = GameType.TicTacToe;
                await repository.GetAvailableGame(gameType);

                A.CallTo(() => repository.GetAvailableGame(gameType)).MustHaveHappened(Repeated.Exactly.Once);
            }
        }

        public class get_game_status_should : GameService_specs
        {
            [Fact]
            public async Task returns_game_status()
            {
                var gameId = Guid.NewGuid();
                var game = TicTacToeApi.Create();
                A.CallTo(() => repository.GetById(gameId)).Returns(game);

                var gameStatus = await gameService.GetGameStatus(gameId, userToken);

                gameStatus.Should().NotBeNull();
                gameStatus.Status.Should().Be(Status.Created);
                gameStatus.LastState.Should().Be(game.LastGameState);
            }
        }

        public class play_should : GameService_specs
        {
            private readonly Guid gameId;
            private readonly Game game;

            public play_should()
            {
                gameId = Guid.NewGuid();
                game = TicTacToeApi.Create();
                A.CallTo(() => repository.GetById(gameId)).Returns(game);
            }

            public class raise_invalid_command_exception : play_should
            {
                [Fact]
                public async Task when_command_is_null()
                {
                    var exception = await Assert.ThrowsAsync<InvalidCommandException>(() => gameService.Play(gameId, null, userToken));
                    exception.Message.Should().Be("Play command is required");
                }

                [Fact]
                public async Task when_instruction_is_null()
                {
                    var command = new Play();
                    var exception = await Assert.ThrowsAsync<InvalidCommandException>(() => gameService.Play(gameId, command, userToken));
                    exception.Message.Should().Be("Instruction is required");
                }
            }

            [Fact]
            public async Task execute_the_instruction()
            {
                var command = new Play { Instruction = 1 };
                game.WithPlayer2(userToken);

                await gameService.Play(gameId, command, userToken);

                A.CallTo(() => repository.GetById(gameId)).MustHaveHappened(Repeated.Exactly.Once);
                A.CallTo(() => repository.Save(game)).MustHaveHappened(Repeated.Exactly.Once);
            }
        }

    }
}
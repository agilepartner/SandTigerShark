using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Options;
using SandTigerShark.GameServer.Services.Configurations;
using SandTigerShark.GameServer.Services.Games;
using SandTigerShark.GameServer.Services.Http;
using SandTigerShark.GameServer.Services.TicTacToes;
using System;
using Xunit;

namespace SandTigerShark.GameServer.Tests.Games
{
    public class GameFactory_specs
    {
        protected IGameFactory gameFactory;
        protected Guid userToken;

        public GameFactory_specs()
        {
            userToken = Guid.NewGuid();
            gameFactory = new GameFactory(A.Fake<IRestProxy>(), A.Fake<IOptions<AzureConfig>>());
        }

        public class create_should : GameFactory_specs
        {
            [Fact]
            public void instanciate_a_new_tic_tac_toe_game()
            {
                var game = gameFactory.Create(GameType.TicTacToe, userToken);

                game.Should().BeOfType<TicTacToe>();
                game.Player1.Should().Be(userToken);
            }
        }
    }
}

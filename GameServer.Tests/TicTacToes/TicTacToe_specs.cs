using FakeItEasy;
using FluentAssertions;
using SandTigerShark.GameServer.Services.Configurations;
using SandTigerShark.GameServer.Services.Exceptions;
using SandTigerShark.GameServer.Services.Games;
using SandTigerShark.GameServer.Services.Http;
using SandTigerShark.GameServer.Services.TicTacToes;
using SandTigerShark.GameServer.Tests.Games;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SandTigerShark.GameServer.Tests.TicTacToes
{
    public class TicTacToe_specs
    {
        private readonly IRestProxy restProxy;
        private readonly TicTacToe ticTacToe;
        private readonly AzureConfig configuration;
        private readonly Guid creatorToken;

        public TicTacToe_specs()
        {
            configuration = new AzureConfig { TicTacToe = "" };
            restProxy = A.Fake<IRestProxy>();
            creatorToken = Guid.NewGuid();

            ticTacToe = new TicTacToe(restProxy, configuration, creatorToken);
        }

        public class create_should : TicTacToe_specs
        {
            [Fact]
            public void add_the_player_to_the_game()
            {
                ticTacToe.Player1.Should().Be(creatorToken);
                ticTacToe.Player2.Should().BeNull();
                ticTacToe.IsAvailable().Should().BeTrue();
            }

            [Fact]
            public void create_should_instanciate_the_board()
            {
                ticTacToe.LastGameState.Should().BeOfType<int[]>();
                int[] board = (int[])ticTacToe.LastGameState;

                board.Should().HaveCount(9);
                board.ToList().ForEach(c => c.Should().Be(0));
            }
        }

        public class play_should : TicTacToe_specs
        {
            public class throw_an_exception : play_should
            {
                [Fact]
                public async void when_a_player_outside_of_the_game_try_to_play()
                {
                    var player2 = Guid.NewGuid();
                    var outsidePlayer = Guid.NewGuid();
                    ticTacToe.WithPlayer2(player2);

                    await Assert.ThrowsAsync<NotAuthorizedException>(() => ticTacToe.Play(new Services.Commands.Play(), outsidePlayer));
                }

                [Fact]
                public async void when_instruction_is_null()
                {
                    var command = new Services.Commands.Play();
                    await Assert.ThrowsAsync<InvalidCommandException>(() => ticTacToe.Play(command, creatorToken));
                }

                [Fact]
                public async void when_instruction_is_not_an_int()
                {
                    var command = new Services.Commands.Play
                    {
                        Instruction = "NotAnInt"
                    };
                    await Assert.ThrowsAsync<InvalidCommandException>(() => ticTacToe.Play(command, creatorToken));
                }

                [Fact]
                public async void when_only_one_player()
                {
                    var command = new Services.Commands.Play
                    {
                        Instruction = 4
                    };
                    await Assert.ThrowsAsync<InvalidCommandException>(() => ticTacToe.Play(command, creatorToken));
                }
            }
        }

        [Fact]
        public async Task it_should_send_a_valid_command_to_azure()
        {
            var command = new Services.Commands.Play()
            {
                Instruction = 1
            };
            ticTacToe.WithPlayer2();

            await ticTacToe.Play(command, creatorToken);

            A.CallTo(() => restProxy.PostAsync(A<string>.Ignored,
                A<Play>.That.Matches(p => p.Position == 1 && p.Player == 1), 
                null)).MustHaveHappened(Repeated.Exactly.Once);
            ticTacToe.State.Should().Be(Status.InProgress);
        }
    }
}

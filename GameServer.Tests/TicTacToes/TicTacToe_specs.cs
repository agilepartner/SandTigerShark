using FakeItEasy;
using SandTigerShark.GameServer.Services.Configurations;
using SandTigerShark.GameServer.Services.Http;
using SandTigerShark.GameServer.Services.TicTacToes;
using System;
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

        /*[Fact]
        public async void it_should_throw_an_ArgumentNullException_for_a_null_command()
        {
            var type = typeof(ArgumentNullException);
            await Assert.ThrowsAsync(type, async() => await ticTacToe.Play(null, creatorToken));
        }

        [Fact]
        public async void it_should_send_a_valid_command()
        {
            var command = new Services.Commands.Play();
            await ticTacToe.Play(command, creatorToken);

            A.CallTo(() => restProxy.PostAsync(configuration.TicTacToe, 
                A<Play>.That.Matches(p => p.Player == 1 && p.Position == 1), null)).MustHaveHappened();
        }*/
    }
}
using FakeItEasy;
using Microsoft.Extensions.Options;
using SandTigerShark.GameServer.Services.Commands;
using SandTigerShark.GameServer.Services.Configurations;
using SandTigerShark.GameServer.Services.Http;
using SandTigerShark.GameServer.Services.TicTacToe;
using System;
using Xunit;

namespace SandTigerShark.GameServer.Tests.TicTacToe
{
    public class TicTacToe_specs
    {
        private readonly IRestProxy restProxy;
        private readonly ITicTacToeService ticTacToeService;
        private readonly AzureConfig configuration;

        public TicTacToe_specs()
        {
            configuration = new AzureConfig { TicTacToe = "" };
            restProxy = A.Fake<IRestProxy>();

            var options = A.Fake<IOptions<AzureConfig>>();
            A.CallTo(() => options.Value).Returns(configuration);

            ticTacToeService = new TicTacToeService(restProxy, options);
        }

        [Fact]
        public async void it_should_throw_an_ArgumentNullException_for_a_null_command()
        {
            var type = typeof(ArgumentNullException);
            await Assert.ThrowsAsync(type, async() => await ticTacToeService.Play(null));
        }

        [Fact]
        public async void it_should_send_a_valid_command()
        {
            var command = new Play();
            await ticTacToeService.Play(command);
        }
    }
}
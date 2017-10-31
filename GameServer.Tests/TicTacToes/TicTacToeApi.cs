using FakeItEasy;
using SandTigerShark.GameServer.Services.Configurations;
using SandTigerShark.GameServer.Services.Http;
using SandTigerShark.GameServer.Services.TicTacToes;
using System;

namespace SandTigerShark.GameServer.Tests.TicTacToes
{
    internal static class TicTacToeApi
    {
        public static TicTacToe Create(
            IRestProxy restProxy = null,
            AzureConfig configuration = null,
            Guid? userToken = null)
        {
            return new TicTacToe(
                restProxy ?? A.Fake<IRestProxy>(),
                configuration ?? new AzureConfig(),
                userToken ?? Guid.NewGuid());
        }
    }
}
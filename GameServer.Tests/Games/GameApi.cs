using SandTigerShark.GameServer.Services.Games;
using SandTigerShark.GameServer.Services.Utils;
using System;

namespace SandTigerShark.GameServer.Tests.Games
{
    internal static class GameApi
    {
        public static Game WithPlayer2(this Game game, Guid? player2 = null)
        {
            game.AddPlayer(player2 ?? Guid.NewGuid());
            return game;
        }

        public static Game InState(this Game game, Status status = Status.InProgress)
        {
            Reflect<Game>.SetPropertyValue(game, "State", status);
            return game;
        }

        public static Game Won(this Game game, Guid? playerToken = null)
        {
            game.InState(Status.GameOver);
            Reflect<Game>.SetPropertyValue(game, "Winner", playerToken.HasValue ? playerToken : game.Player1);
            return game;
        }
    }
}
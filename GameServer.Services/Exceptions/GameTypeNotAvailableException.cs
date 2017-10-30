using System;
using System.Runtime.Serialization;

namespace SandTigerShark.GameServer.Services.Exceptions
{
    public class GameTypeNotAvailableException : Exception
    {
        public GameTypeNotAvailableException(string gameType)
            : base($"The game {gameType} is not available")
        {
        }
    }
}
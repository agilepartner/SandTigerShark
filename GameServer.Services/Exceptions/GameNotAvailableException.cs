using System;
using System.Runtime.Serialization;

namespace SandTigerShark.GameServer.Services.Exceptions
{
    public class GameNotAvailableException : Exception
    {
        public GameNotAvailableException(Guid gameId)
            : base($"The game {gameId} is not available")
        {
        }
    }
}
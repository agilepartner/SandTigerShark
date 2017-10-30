using System;

namespace SandTigerShark.GameServer.Services.Exceptions
{
    public class InvalidCommandException : Exception
    {
        public InvalidCommandException(string message)
            : base(message)
        {

        }
    }
}

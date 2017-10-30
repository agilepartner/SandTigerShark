using System;

namespace SandTigerShark.GameServer.Services.Commands
{
    public class Play
    {
        public Guid GameId { get; set; }
        public object Command { get; set; }
    }
}
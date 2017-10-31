using System;

namespace SandTigerShark.GameServer.Services.Dtos
{
    public class User
    {
        public string Name { get; set; }
        public Guid Token { get; set; }
    }
}
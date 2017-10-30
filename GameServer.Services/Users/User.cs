using System;

namespace SandTigerShark.GameServer.Services.Users
{
    public class User
    {
        public string Name { get; }
        public Guid Token { get; }

        public User(string name)
        {
            Name = name;
            Token = Guid.NewGuid();
        }
    }
}
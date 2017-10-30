using SandTigerShark.GameServer.Services.Commands;
using SandTigerShark.GameServer.Services.Exceptions;
using System;
using System.Threading.Tasks;

namespace SandTigerShark.GameServer.Services.Users
{
    internal class UserService : IUserService
    {
        private readonly IUserRepository repository;

        public UserService(IUserRepository repository)
        {
            this.repository = repository;
        }

        public Task<Guid> CreateUser(CreateUser command)
        {
            if (command == null)
            {
                throw new InvalidCommandException($"CreateUser command is required");
            }

            if (string.IsNullOrEmpty(command.UserName))
            {
                throw new InvalidCommandException($"{nameof(command.UserName)} is required in order to create a new user");
            }

            var user = new User(command.UserName);
            repository.Save(user);

            return Task.FromResult(user.Token);
        }

        public async Task<User> GetUser(Guid userToken)
        {
            return await repository.GetByToken(userToken);
        }
    }
}
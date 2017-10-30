using SandTigerShark.GameServer.Services.Exceptions;
using System;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace SandTigerShark.GameServer.Services.Users
{
    internal class UserRepository : IUserRepository
    {
        private ConcurrentDictionary<Guid, User> users = new ConcurrentDictionary<Guid, User>();

        public Task Save(User user)
        {
            if (users.Values.ToImmutableList()
                    .Where(u => u.Name == user.Name)
                    .Any())
            {
                throw new EntityAlreadyExistsException($"User '{user.Name}' already exists");
            }
            users.AddOrUpdate(user.Token, user, (id, u) => u);
            return Task.FromResult(true);
        }

        public Task<User> GetByToken(Guid userToken)
        {
            User user = null;

            if (!users.TryGetValue(userToken, out user))
            {
                throw new NotFoundException($"No user found with token : {userToken}");
            }
            return Task.FromResult(user);
        }
    }
}
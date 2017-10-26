using GameServer.Repositories;
using SandTigerShark.GameServer.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace SandTigerShark.Repositories
{

    public class UserRepository : IUserRepository
    {

        private static readonly ImmutableDictionary<string, Guid> userNameToToken =
            ImmutableDictionary <string, Guid>.Empty
            .Add("User1", Guid.NewGuid())
            .Add("User2", Guid.NewGuid());


        public Task<Guid> GetUserToken(string userName)
        {
            var userToken = userNameToToken.GetValueOrDefault(userName);
            if (userToken == null || Guid.Empty.Equals(userToken))
            {
                throw new NotFoundException();
            }
            return Task.FromResult(userToken);
        }


    }

}
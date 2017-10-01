using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace SandTigerShark.Repositories
{

    public class UserRepository
    {

        private static readonly ImmutableDictionary<string, Guid> userNameToToken =
            ImmutableDictionary <string, Guid>.Empty
            .Add("User1", Guid.NewGuid())
            .Add("User2", Guid.NewGuid());


        public Guid GetUserToken(string userName)
        {
            return userNameToToken.GetValueOrDefault(userName);
        }


    }

}
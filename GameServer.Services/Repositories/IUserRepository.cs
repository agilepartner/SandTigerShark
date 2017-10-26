using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SandTigerShark.GameServer.Repositories
{
    public interface IUserRepository
    {
        Task<Guid> GetUserToken(string userName);
    }
}

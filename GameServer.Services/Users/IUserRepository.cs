using System;
using System.Threading.Tasks;

namespace SandTigerShark.GameServer.Services.Users
{
    public interface IUserRepository
    {
        Task Save(User user);
        Task<User> GetByToken(Guid userToken);
    }
}
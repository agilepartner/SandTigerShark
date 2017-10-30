using SandTigerShark.GameServer.Services.Commands;
using System;
using System.Threading.Tasks;

namespace SandTigerShark.GameServer.Services.Users
{
    public interface IUserService
    {
        Task<Guid> CreateUser(CreateUser command);
        Task<User> GetUser(Guid userToken);
    }
}
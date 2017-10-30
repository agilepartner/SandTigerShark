using Microsoft.Extensions.DependencyInjection;
using SandTigerShark.GameServer.Services.Games;
using SandTigerShark.GameServer.Services.Http;
using SandTigerShark.GameServer.Services.Users;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("SandTigerShark.GameServer.Tests")]
namespace SandTigerShark.GameServer.Services
{
    public static class Module
    {
        public static void Bootstrap(IServiceCollection services)
        {
            services.AddScoped<IRestProxy, RestProxy>()
                    .AddSingleton<IGameRepository, GameRepository>()
                    .AddSingleton<IUserRepository, UserRepository>()
                    .AddScoped<IGameFactory, GameFactory>()
                    .AddScoped<IGameService, GameService>()
                    .AddScoped<IUserService, UserService>();
        }
    }
}
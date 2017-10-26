using GameServer.Services.Repositories;
using Microsoft.Extensions.DependencyInjection;
using SandTigerShark.GameServer.Repositories;
using SandTigerShark.GameServer.Services.Http;
using SandTigerShark.GameServer.Services.TicTacToe;
using SandTigerShark.Repositories;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("SandTigerShark.GameServer.Tests")]
namespace SandTigerShark.GameServer.Services
{
    public static class Module
    {
        public static void Bootstrap(IServiceCollection services)
        {
            services.AddScoped<IGameRepository, GameRepository>()
                    .AddScoped<IRestProxy, RestProxy>()
                    .AddScoped<ITicTacToeService, TicTacToeService>()
                    .AddScoped<IGameRepository, GameRepository>()
                    .AddScoped<IUserRepository, UserRepository>();
        }
    }
}
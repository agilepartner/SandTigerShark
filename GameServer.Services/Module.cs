using GameServer.Services.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace SandTigerShark.GameServer.Services
{
    public static class Module
    {
        public static void Bootstrap(IServiceCollection services)
        {
            services.AddScoped<IGameRepository, GameRepository>();
        }
    }
}
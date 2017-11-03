using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace SandTigerShark.TicTacToe.App
{
    class Program
    {
        private static IServiceProvider serviceProvider;
        private static ConsoleColor defaultConsoleColor = ConsoleColor.White;

        static void Main(string[] args)
        {
            Console.ForegroundColor = defaultConsoleColor;
            var configuration = BuildConfiguration();
            serviceProvider = ConfigureServices(configuration);

            var resultCode = Start().Result;
            Console.ReadKey();
        }

        private async static Task<int> Start()
        {
            var userRepository = serviceProvider.GetService<UserRepository>();
            var userName = AskUserName();
            var userToken = await userRepository.Create(userName);
            WriteInfo($"User-Token:{userToken}");

            AddUserTokenInProxy(userToken);

            var ticTacToe = serviceProvider.GetService<TicTacToe>();
            await ticTacToe.Start();
            WriteInfo($"Game id:{ticTacToe.Id}");

            while (ticTacToe.InProgress)
            {
                int position = AskWhereToPlay();
                var result = await ticTacToe.Play(position);

                if (result.Success)
                {
                    WriteBoard(Mapper.ToVisualBoard(result.Board));
                }
                else
                {
                    WriteError(result.Message);
                }
            }
            return 0;
        }


        private static void AddUserTokenInProxy(Guid userToken)
        {
            var restProxy = serviceProvider.GetService<IRestProxy>();
            restProxy.AddHttpHeader("user-token", userToken.ToString());
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var configuration = new ConfigurationBuilder();
            configuration.AddJsonFile("appsettings.json");

            return configuration.Build();
        }

        private static IServiceProvider ConfigureServices(IConfigurationRoot configuration)
        {
            var services = new ServiceCollection();

            services.AddOptions()
                    .Configure<HttpClientConfig>(configuration.GetSection("Http"))
                    .Configure<GameConfig>(configuration.GetSection("Game"))
                    .AddSingleton<IConfiguration>(configuration)
                    .AddSingleton<IRestProxy, RestProxy>()
                    .AddSingleton<TicTacToeRepository>()
                    .AddSingleton<UserRepository>()
                    .AddTransient<TicTacToe>();

            return services.BuildServiceProvider();
        }

        private static void WriteInfo(string info)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(info);
            Console.ForegroundColor = defaultConsoleColor;
        }

        private static void WriteError(string error)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(error);
            Console.ForegroundColor = defaultConsoleColor;
        }

        private static string AskUserName()
        {
            Console.WriteLine("Enter your user name :");
            return Console.ReadLine();
        }

        private static int AskWhereToPlay()
        {
            Console.WriteLine("Play at position :");
            return int.Parse(Console.ReadLine());
        }

        private static void WriteBoard(string[] board)
        {
            Console.ForegroundColor = ConsoleColor.Green;

            foreach (var line in board)
            {
                Console.WriteLine(line);
            }

            Console.ForegroundColor = defaultConsoleColor;
        }
    }
}
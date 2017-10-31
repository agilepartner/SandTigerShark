using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace SandTigerShark.GameServer.Tests.Integrations
{
    public class WebApiTest<TStartup> : IDisposable
        where TStartup : class
    {
        private readonly TestServer server;

        public WebApiTest(
            string environment = "Test",
            string contentRoot = null)
        {
            var builder = new WebHostBuilder()
                                .UseContentRoot(contentRoot ?? Directory.GetCurrentDirectory())
                                .UseStartup<TStartup>()
                                .UseEnvironment(environment)
                                .UseConfiguration(new ConfigurationBuilder()
                                    .SetBasePath(contentRoot ?? Directory.GetCurrentDirectory())
                                    .AddJsonFile("appsettings.json")
                                    .Build());

            server = new TestServer(builder);
            Client = server.CreateClient();
        }

        public HttpClient Client { get; }

        public async Task<HttpClient> GetClientWithUserToken(string userName = "John Doe")
        {
            return await server.CreateClient().WithUserToken(userName);
        }

        public void Dispose()
        {
            Client.Dispose();
            server.Dispose();
        }
    }
}

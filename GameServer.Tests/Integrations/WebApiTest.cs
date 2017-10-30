using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.IO;
using System.Net.Http;

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
                                .UseEnvironment(environment);

            server = new TestServer(builder);
            Client = server.CreateClient();
            Client.DefaultRequestHeaders.Add("user-token", Guid.NewGuid().ToString());
        }

        public HttpClient Client { get; }

        public void Dispose()
        {
            Client.Dispose();
            server.Dispose();
        }
    }
}

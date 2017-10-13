using FluentAssertions;
using Xunit;

namespace SandTigerShark.GameServer.Tests.Integrations
{
    public class Sandbox_specs : WebApiTest<Startup>
    {
        [Fact]
        public async void Get()
        {
            using (var response = await Client.GetAsync("api/Sandbox"))
            {
                var result = await response.Content.ReadAsStringAsync();
                result.Should().Be("Hello World");
            }
        }
    }
}
using FluentAssertions;
using SandTigerShark.GameServer.Services.Commands;
using SandTigerShark.GameServer.Services.Users;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace SandTigerShark.GameServer.Tests.Integrations
{
    public class UsersController_specs
    {
        private const string baseUrl = "api/users";

        public class create_user_should : WebApiTest<Startup>
        {
            public class returns_created : create_user_should
            {
                [Fact]
                public async Task when_user_name_does_not_exist()
                {
                    var command = new CreateUser { UserName = "@yot57" };

                    using (var response = await Client.PostAsync($"{baseUrl}", command))
                    {
                        response.StatusCode.Should().Be(HttpStatusCode.Created);

                        var result = await response.Content.ReadAsync<Guid?>();
                        result.Should().NotBeNull();
                        result.Should().NotBeEmpty();
                        response.Headers.Location.OriginalString.Should().Be($"api/users/{result}");
                    }
                }
            }

            public class returns_method_not_allowed : create_user_should
            {
                [Fact]
                public async Task when_user_already_exists()
                {
                    var command = new CreateUser { UserName = "@yot57" };

                    await Client.PostAsync($"{baseUrl}", command);

                    using (var response = await Client.PostAsync($"{baseUrl}", command))
                    {
                        response.StatusCode.Should().Be(HttpStatusCode.MethodNotAllowed);
                    }
                }
            }
        }

        public class get_a_user : WebApiTest<Startup>
        {
            public class that_does_not_exist : get_a_user
            {
                [Fact]
                public async void then_a_not_found_http_status_is_received()
                {
                    var unexistingUserToken = Guid.NewGuid();

                    using (var response = await Client.GetAsync($"{baseUrl}/{unexistingUserToken}"))
                    {
                        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
                    }
                }
            }

            public class that_exists : get_a_user
            {
                [Fact]
                public async void then_i_get_a_user()
                {
                    var command = new CreateUser { UserName = "@yot57" };

                    using (var postResponse = await Client.PostAsync($"{baseUrl}", command))
                    {
                        var userToken = await postResponse.Content.ReadAsync<Guid>();

                        using (var getResponse = await Client.GetAsync($"api/users/{userToken}"))
                        {
                            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

                            var user = await getResponse.Content.ReadAsync<Services.Dtos.User>();
                            user.Name.Should().Be(command.UserName);
                            user.Token.Should().Be(userToken);
                        }
                    }
                }
            }
        }
    }
}

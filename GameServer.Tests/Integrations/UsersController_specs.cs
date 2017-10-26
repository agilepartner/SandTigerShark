using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Xunit;

namespace SandTigerShark.GameServer.Tests.Integrations
{
    public class UsersController_specs
    {

        public class when_I_request_a_user_token_that_does_exist : WebApiTest<Startup>
        {

            [Fact]
            public async void then_I_get_a_not_empty_token()
            {
                using (var response = await Client.GetAsync("api/users/token/userthatdoesntexist"))
                {
                    var result = await response.Content.ReadAsStringAsync();
                    response.Content.Should().NotBe(Guid.Empty.ToString());
                }

            }
        }

        public class when_I_request_a_user_token_that_does_not_exist : WebApiTest<Startup>
        {

            [Fact]
            public async void then_I_get_a_not_found_status_code()
            {
                using (var response = await Client.GetAsync("api/users/token/userthatdoesntexist"))
                {
                    var result = await response.Content.ReadAsStringAsync();
                    response.StatusCode.Should().Be(HttpStatusCode.NotFound);
                }

            }
        }
    }
}

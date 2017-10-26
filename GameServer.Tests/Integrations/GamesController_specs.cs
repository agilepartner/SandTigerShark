﻿using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Xunit;

namespace SandTigerShark.GameServer.Tests.Integrations
{
    public class GamesController_specs
    {
        public class when_I_request_a_game_status_that_does_not_exists : WebApiTest<Startup>
        {

            /*
            [Fact]
            public async void then_I_get_a_not_found_status_code()
            {
                using (var response = await Client.GetAsync("api/games/gameState/tarteenpion"))
                {
                    var result = await response.Content.ReadAsStringAsync();
                    response.StatusCode.Should().Be(HttpStatusCode.NotFound);
                }

            }
            */
        }

        public class when_there_is_not_available_game : WebApiTest<Startup>
        {
            [Fact]
            public async void then_I_cant_get_a_token()
            {
                
                using (var response = await Client.GetAsync("api/games/games"))
                {
                    var result = await response.Content.ReadAsStringAsync();
                    response.StatusCode.Should().Be(HttpStatusCode.NotFound);
                }

            }
        }

    }
}
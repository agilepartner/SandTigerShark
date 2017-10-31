using FakeItEasy;
using FluentAssertions;
using SandTigerShark.GameServer.Services.Commands;
using SandTigerShark.GameServer.Services.Exceptions;
using SandTigerShark.GameServer.Services.Users;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SandTigerShark.GameServer.Tests.Users
{
    public class UserService_specs
    {
        private readonly IUserRepository repository;
        private readonly UserService userService;

        public UserService_specs()
        {
            repository = A.Fake<IUserRepository>();
            userService = new UserService(repository);
        }

        public class create_user_should : UserService_specs
        {
            public class raise_invalid_command_exception : create_user_should
            {
                [Fact]
                public async Task when_command_is_null()
                {
                    var exception = await Assert.ThrowsAsync<InvalidCommandException>(() => userService.CreateUser(null));
                    exception.Message.Should().Be("CreateUser command is required");
                }

                [Fact]
                public async Task when_user_name_is_null_or_empty()
                {
                    var command = new CreateUser();

                    var exception = await Assert.ThrowsAsync<InvalidCommandException>(() => userService.CreateUser(command));
                    exception.Message.Should().Be("UserName is required in order to create a new user");
                }
            }

            [Fact]
            public async Task create_and_store_a_new_user()
            {
                var command = new CreateUser { UserName = "yot57" };

                var newUserId = await userService.CreateUser(command);

                A.CallTo(() => repository.Save(A<User>.That.Matches(u => u.Name == command.UserName))).MustHaveHappened(Repeated.Exactly.Once);
                newUserId.Should().NotBeEmpty();
            }
        }

        public class get_user_should : UserService_specs
        {
            [Fact]
            public async Task returns_user_from_repository()
            {
                var userToken = Guid.NewGuid();

                await userService.GetUser(userToken);

                A.CallTo(() => repository.GetByToken(userToken)).MustHaveHappened(Repeated.Exactly.Once);
            }
        }
    }
}

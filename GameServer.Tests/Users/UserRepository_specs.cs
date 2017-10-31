using FluentAssertions;
using SandTigerShark.GameServer.Services.Exceptions;
using SandTigerShark.GameServer.Services.Users;
using SandTigerShark.GameServer.Services.Utils;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SandTigerShark.GameServer.Tests.Users
{
    public class UserRepository_specs
    {
        private readonly UserRepository repository;

        public UserRepository_specs()
        {
            repository = new UserRepository();
        }

        public class create_user_should : UserRepository_specs
        {
            public class raise_an_entity_already_exists_exception : create_user_should
            {
                [Fact]
                public async Task when_a_user_already_exists_with_a_given_name()
                {
                    var existingUserName = "existingUser";
                    InitializeUsersInRepository(existingUserName);
                    var existingUser = new User(existingUserName);

                    var exception = await Assert.ThrowsAsync<EntityAlreadyExistsException>(() => repository.Save(existingUser));
                    exception.Message.Should().Be("User 'existingUser' already exists");
                }
            }

            [Fact]
            public async Task returns_the_game_for_a_given_id()
            {
                var newUser = new User("John McClane");
                await repository.Save(newUser);

                var gamesInRepository = Reflect<UserRepository>.GetFieldValue<ConcurrentDictionary<Guid, User>>(repository, "users");

                gamesInRepository.Should().HaveCount(1);
                gamesInRepository.Single().Value.Should().Be(newUser);
            }
        }

        public class get_by_token_should : UserRepository_specs
        {
            public class raise_a_not_found_exception : create_user_should
            {
                [Fact]
                public async Task when_no_user_found_for_the_given_token()
                {
                    var userToken = Guid.NewGuid();

                    var exception = await Assert.ThrowsAsync<NotFoundException>(() => repository.GetByToken(userToken));
                    exception.Message.Should().StartWith("No user found with token :");
                }
            }

            [Fact]
            public async Task returns_the_user_for_a_given_token()
            {
                var existingUser = new User("existingUser");
                InitializeUsersInRepository(existingUser);

                var foundUser = await repository.GetByToken(existingUser.Token);

                foundUser.Should().Be(existingUser);
            }
        }

        protected void InitializeUsersInRepository(params string[] userNames)
        {
            var users = userNames.Select(userName => new User(userName)).ToArray();
            InitializeUsersInRepository(users);

        }

        protected void InitializeUsersInRepository(params User[] users)
        {
            var userDictionary = users.ToDictionary(u => u.Token, u => u);
            Reflect<UserRepository>.SetFieldValue(repository, "users", new ConcurrentDictionary<Guid, User>(userDictionary));
        }
    }
}

using SandTigerShark.Repositories;
using Xunit;

namespace SandTigerShark.GameServer.Tests
{

    public class UserRepositoryTest
    {

        [Theory]
        [InlineData("User1")]
        [InlineData("User2")]
        [InlineData("UnkonwnUser")]
        public void UserRepositoryCannotReturnNullToken(string userName)
        {
            Assert.NotNull(new UserRepository().GetUserToken(userName));
        }

        [Theory]
        [InlineData("User1")]
        [InlineData("User2")]
        [InlineData("UnkonwnUser")]
        public void UserRepositoryReturnTheSameTokenForAGivenUser(string userName)
        {
            Assert.Equal(
                new UserRepository().GetUserToken(userName),
                new UserRepository().GetUserToken(userName));
        }


    }

}
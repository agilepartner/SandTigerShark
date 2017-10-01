using Xunit;

namespace SandTigerShark.Repositories
{

    public class UserRepositoryTest
    {

        [Fact]
        public void PassingTest()
        {
            Assert.Equal(4, Add(2, 2));
        }

        [Fact]
        public void FailingTest()
        {
            Assert.Equal(5, Add(2, 2));
        }

        int Add(int x, int y)
        {
            return x + y;
        }

        [Theory]
        [InlineData("User1")]
        [InlineData("User2")]
        [InlineData("UnkonwnUser")]
        [InlineData(null)]
        public void UserRepositoryCannotReturnNullToken(string userName)
        {
            Assert.NotNull(new UserRepository().GetUserToken(userName));
        }


    }

}
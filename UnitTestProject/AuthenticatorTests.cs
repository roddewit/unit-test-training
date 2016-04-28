using NUnit.Framework;
using Refactoring;
using System.Collections.Generic;

namespace UnitTestProject
{
    [TestFixture]
    class AuthenticatorTests
    {
        private List<User> createTestUserList()
        {
            var testUser = new User {Name = "user1", Password = "pwd123", Balance = 99.99};
            var users = new List<User> {testUser};

            return users;
        }

        [Test]
        public void Test_UserIsValid()
        {
            //Arrange
            var users = createTestUserList();
            var authenticator = new Authenticator(users);

            //Act
            var authenticatedUser = authenticator.Authenticate("user1", "pwd123");

            //Assert
            Assert.AreSame(users[0], authenticatedUser);
        }

        [Test]
        public void Test_UsernameIsInvalid()
        {
            //Arrange
            var users = createTestUserList();
            var authenticator = new Authenticator(users);

            //Act
            var authenticatedUser = authenticator.Authenticate("user2", "pwd123");

            //Assert
            Assert.Null(authenticatedUser);
        }

        [Test]
        public void Test_PasswordIsInvalid()
        {
            //Arrange
            var users = createTestUserList();
            var authenticator = new Authenticator(users);

            //Act
            var authenticatedUser = authenticator.Authenticate("user1", "pwd456");

            //Assert
            Assert.Null(authenticatedUser);
        }

        [Test]
        public void Test_UsernameIsNull()
        {
            //Arrange
            var users = createTestUserList();
            var authenticator = new Authenticator(users);

            //Act
            var authenticatedUser = authenticator.Authenticate(null, "pwd123");

            //Assert
            Assert.Null(authenticatedUser);
        }

        [Test]
        public void Test_PasswordIsNull()
        {
            //Arrange
            var users = createTestUserList();
            var authenticator = new Authenticator(users);

            //Act
            var authenticatedUser = authenticator.Authenticate("user1", null);

            //Assert
            //The authenticator will still return the correct user even if the password is null
            Assert.AreSame(users[0], authenticatedUser);
        }
    }
}

using NUnit.Framework;
using Refactoring;
using System.Collections.Generic;

namespace UnitTestProject
{
    [TestFixture]
    class AuthenticatorTests
    {
        [Test]
        public void Test_EmptyUserList()
        {
            //Arrange
            List<User> users = new List<User>();
            Authenticator authenticator = new Authenticator(users);

            //Act
            User authenticatedUser = authenticator.Authenticate("username", "password");

            //Assert
            Assert.Null(authenticatedUser);
            Assert.Pass("No assertion really necessary here");
        }

        [Test]
        public void Test_ValidUser()
        {
            //Arrange
            User user = new User("username", "password", 0);
            List<User> users = new List<User>();
            users.Add(user);

            Authenticator authenticator = new Authenticator(users);

            //Act
            User authenticatedUser = authenticator.Authenticate("username", "password");

            //Assert
            Assert.NotNull(authenticatedUser);
            Assert.AreEqual("username", authenticatedUser.Name);
        }

        [Test]
        public void Test_MultipleUsers()
        {
            //Arrange
            User user1 = new User("username1", "password1", 0);
            User user2 = new User("username2", "password2", 0);
            User user3 = new User("username3", "password3", 0);

            List<User> users = new List<User>();
            users.Add(user1);
            users.Add(user2);
            users.Add(user3);

            Authenticator authenticator = new Authenticator(users);

            //Act
            User authenticatedUser = authenticator.Authenticate("username2", "password2");

            //Assert
            Assert.NotNull(authenticatedUser);
            Assert.AreEqual("username2", authenticatedUser.Name);
        }

        [Test]
        public void Test_DuplicateUser()
        {
            //Arrange
            User user1 = new User("username", "password", 0);
            User user2 = new User("username", "password", 0);

            List<User> users = new List<User>();
            users.Add(user1);
            users.Add(user2);

            Authenticator authenticator = new Authenticator(users);

            //Act
            User authenticatedUser = authenticator.Authenticate("username", "password");

            //Assert
            Assert.NotNull(authenticatedUser);
            Assert.AreEqual("username", authenticatedUser.Name);
        }

        [Test]
        public void Test_InvalidUsername()
        {
            //Arrange
            User user = new User("username", "password", 0);
            List<User> users = new List<User>();
            users.Add(user);

            Authenticator authenticator = new Authenticator(users);

            //Act
            User authenticatedUser = authenticator.Authenticate("wrong_username", "password");

            //Assert
            Assert.Null(authenticatedUser);
        }

        [Test]
        public void Test_EmptyUsername()
        {
            //Arrange
            User user = new User("username", "password", 0);
            List<User> users = new List<User>();
            users.Add(user);

            Authenticator authenticator = new Authenticator(users);

            //Act
            User authenticatedUser = authenticator.Authenticate("", "password");

            //Assert
            Assert.Null(authenticatedUser);
        }

        [Test]
        public void Test_NullUsername()
        {
            //Arrange
            User user = new User("username", "password", 0);
            List<User> users = new List<User>();
            users.Add(user);

            Authenticator authenticator = new Authenticator(users);

            //Act
            User authenticatedUser = authenticator.Authenticate(null, "password");

            //Assert
            Assert.Null(authenticatedUser);
        }

        [Test]
        public void Test_InvalidPassword()
        {
            //Arrange
            User user = new User("username", "password", 0);
            List<User> users = new List<User>();
            users.Add(user);

            Authenticator authenticator = new Authenticator(users);

            //Act
            User authenticatedUser = authenticator.Authenticate("username", "wrong_password");

            //Assert
            Assert.Null(authenticatedUser);
        }

        [Test]
        public void Test_NullPassword()
        {
            //Arrange
            User user1 = new User("username1", "password1", 0);
            User user2 = new User("username2", "password2", 0);

            List<User> users = new List<User>();
            users.Add(user1);
            users.Add(user2);

            Authenticator authenticator = new Authenticator(users);

            //Act
            User authenticatedUser = authenticator.Authenticate("username2", null);

            //Assert
            Assert.NotNull(authenticatedUser);
            Assert.AreEqual("username2", authenticatedUser.Name);
        }
    }
}
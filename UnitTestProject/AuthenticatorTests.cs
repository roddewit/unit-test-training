using NUnit.Framework;
using Refactoring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject
{
    [TestFixture]
    class AuthenticatorTests
    {
        private User createTestUser(string name, string password, double balance)
        {
            User testUser = new User();
            testUser.Name = name;
            testUser.Password = password;
            testUser.Balance = balance;

            return testUser;
        }

        private Authenticator CreateAuthenticator(User user)
        {
            List<User> testUsers = new List<User>();
            testUsers.Add(user);

            return new Authenticator(testUsers);
        }

        [Test]
        public void Test_AuthenticateExistingUser()
        {
            User user = createTestUser("Test User", "", 100);
            Authenticator authenticator = CreateAuthenticator(user);

            User authenticatedUser = authenticator.Authenticate("Test User", "");
            Assert.AreEqual(authenticatedUser, user);
        }

        [Test]
        public void Test_AuthenticateUserThatDoesNotExist()
        {
            User user = createTestUser("Test User", "", 100);
            Authenticator authenticator = CreateAuthenticator(user);

            User authenticatedUser = authenticator.Authenticate("User", "");
            Assert.AreNotEqual(authenticatedUser, user);

        }
        [Test]
        public void Test_AuthenticateNullUser()
        {
            User user = createTestUser("Test User", "", 100);
            Authenticator authenticator = CreateAuthenticator(user);

            User authenticatedUser = authenticator.Authenticate(null,null);
            Assert.AreNotEqual(authenticatedUser, user);

        }

        [Test]
        public void Test_AuthenticateEmptyStringUser()
        {
            User user = createTestUser("Test User", "", 100);
            Authenticator authenticator = CreateAuthenticator(user);

            User authenticatedUser = authenticator.Authenticate("", "");
            Assert.AreNotEqual(authenticatedUser, user);
        }

        [Test]
        public void Test_AuthenticateWithEmptyUserList()
        {
            List<User> users = new List<User>();
            Authenticator authenticator = new Authenticator(users);

            User authenticatedUser = authenticator.Authenticate("User Test", "");
            Assert.AreNotEqual(authenticatedUser, "User Test", "");

        }

    }
}

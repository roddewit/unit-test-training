using Newtonsoft.Json;
using NUnit.Framework;
using Refactoring;
using System;
using System.Collections.Generic;
using System.IO;
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

        //this test found a bug, having a null list makes authenticate blow up. when it should just not return a user for authentication
        [Test]
        public void Test_AuthenticateReturnsNoUserWhenNullListOfUsers()
        {
            Authenticator authenticator = new Authenticator(null);

            User authenticatedUser = authenticator.Authenticate("test", "test");

            Assert.Null(authenticatedUser);
        }

        [Test]
        public void Test_AuthenticateReturnsNoUserWhenEmptyListOfUsers()
        {
            Authenticator authenticator = new Authenticator(new List<User>());

            User authenticatedUser = authenticator.Authenticate("test", "test");

            Assert.Null(authenticatedUser);
        }

        [Test]
        public void Test_AuthenticateReturnsNoUserWhenUserIsNotInList()
        {
            List<User> users = new List<User>() { createTestUser("Test", "Pass", 0.0) };

            Authenticator authenticator = new Authenticator(users);

            User authenticatedUser = authenticator.Authenticate("test", "test");

            Assert.Null(authenticatedUser);
        }

        [Test]
        public void Test_AuthenticateReturnsNoUserWhenUserGivesIncorrectPassword()
        {
            const string USERNAME = "testUser";
            List<User> users = new List<User>() { createTestUser(USERNAME, "P455", 0.0) };

            Authenticator authenticator = new Authenticator(users);

            User authenticatedUser = authenticator.Authenticate(USERNAME, "wrong");

            Assert.Null(authenticatedUser);
        }

        [Test]
        public void Test_AuthenticateReturnsUserWhenUserGivesCorrectPassword()
        {
            const string USERNAME = "testUser";
            const string PASSWORD = "5uP3R53CUR3!";
            User user = createTestUser(USERNAME, PASSWORD, 0.0);

            Authenticator authenticator = new Authenticator(new List<User>() { user });

            User authenticatedUser = authenticator.Authenticate(USERNAME, PASSWORD);

            Assert.AreSame(user, authenticatedUser);
        }

        [Test]
        public void Test_AuthenticateReturnsUserWhenMultipleUsersExistInList()
        {
            const string USERNAME1 = "testUser1";
            const string USERNAME2 = "testUser2";
            const string PASSWORD = "5uP3R53CUR3!";

            User user1 = createTestUser(USERNAME1, PASSWORD, 0.0);
            User user2 = createTestUser(USERNAME2, PASSWORD, 0.0);

            List<User> users = new List<User> { user1, user2 };

            Authenticator authenticator = new Authenticator(users);

            User authenticatedUser = authenticator.Authenticate(USERNAME2, PASSWORD);

            Assert.AreSame(user2, authenticatedUser);
        }

        //this test found a bug to fix. you should not be able to authenticate with a null password
        [Test]
        public void Test_AuthenticateReturnsNullWhenAuthenticateWithNullPassword()
        {
            List<User> users = new List<User>() { createTestUser("Test", "Pass", 0.0) };

            Authenticator authenticator = new Authenticator(users);

            User authenticatedUser = authenticator.Authenticate("Test", null);

            Assert.Null(authenticatedUser);
        }

        [Test]
        public void Test_AuthenticateReturnsNullWhenAuthenticateWithNullUserName()
        {
            List<User> users = new List<User>() { createTestUser("Test", "Pass", 0.0) };

            Authenticator authenticator = new Authenticator(users);

            User authenticatedUser = authenticator.Authenticate(null, "Pass");

            Assert.Null(authenticatedUser);
        }
        
    }
}

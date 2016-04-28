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

        [Test]
        public void Test_FindValidUser()
        {
            User loggedInUser = null;
            string userName = "Test User1";
            string userPassword = "123456";

            var users = new List<User>();
            users.Add(createTestUser(userName, userPassword, 99.99));

            var authenticator = new Authenticator(users);

            loggedInUser = authenticator.Authenticate(userName, userPassword);

            Assert.AreEqual(userName, loggedInUser.Name);
        }

        [Test]
        public void Test_FindInValidUser()
        {
            User loggedInUser = null;
            string userName = "Test User1";
            string userPassword = "123456";

            var users = new List<User>();
            users.Add(createTestUser(userName, userPassword, 99.99));

            var authenticator = new Authenticator(users);

            loggedInUser = authenticator.Authenticate(userName + "1", userPassword);

            Assert.AreEqual(null, loggedInUser);
        }

        [Test]
        public void Test_FindValidUserWithInvalidPassword()
        {
            User loggedInUser = null;
            string userName = "Test User1";
            string userPassword = "123456";

            var users = new List<User>();
            users.Add(createTestUser(userName, userPassword, 99.99));

            var authenticator = new Authenticator(users);

            loggedInUser = authenticator.Authenticate(userName, userPassword + "123");

            Assert.AreEqual(null, loggedInUser);
        }

        [Test]
        public void Test_ValidateNullPassword()
        {
            User loggedInUser = null;
            string userName = "Test User1";
            string userPassword = "123456";

            var users = new List<User>();
            users.Add(createTestUser(userName, userPassword, 99.99));

            var authenticator = new Authenticator(users);

            loggedInUser = authenticator.Authenticate(userName, null);

            Assert.AreEqual(null, loggedInUser.Name);
        }

        [Test]
        public void Test_ValidateNullUserName()
        {
            User loggedInUser = null;
            string userName = "Test User1";
            string userPassword = "123456";

            var users = new List<User>();
            users.Add(createTestUser(userName, userPassword, 99.99));

            var authenticator = new Authenticator(users);

            loggedInUser = authenticator.Authenticate(null, userPassword);

            Assert.AreEqual(null, loggedInUser);
        }

    }
}

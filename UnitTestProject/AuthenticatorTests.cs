using System;
using NUnit.Framework;
using Refactoring;
using System.Collections.Generic;

namespace UnitTestProject
{
    [TestFixture]
    public class AuthenticatorTests
    {
        List<User> users;
        Authenticator authenticator;

        private User createTestUser(string name, string password, double balance)
        {
            User testUser = new User();
            testUser.Name = name;
            testUser.Password = password;
            testUser.Balance = balance;

            return testUser;
        }

        [SetUp]
        public void Test_CodeSetup()
        {
            users = new List<User>();
            users.Add(createTestUser("Test User A", "pass123", 99.99));
            users.Add(createTestUser("Test User B", "Password01", 99.99));
            users.Add(createTestUser("Test User C", "password", 99.99));
            users.Add(createTestUser("Test User D", "hunter2", 99.99));
            users.Add(createTestUser("Test User E", "f8W&3M9v=", 99.99));

            authenticator = new Authenticator(users);
        }

        [Test]
        public void Test_NullOrEmptyUsername()
        {
            User result;
            result = authenticator.Authenticate("", "password");
            Assert.AreEqual(null, result);
        }

        [Test]
        public void Test_ValidUsername()
        {
            User result;
            result = authenticator.Authenticate("Test User D", "hunter2");
            Assert.AreEqual(users[3], result);
        }

        [Test]
        public void Test_CannotFindUserWithIncorrectUsername()
        {
            User result;
            result = authenticator.Authenticate("Test User X", "pass123");
            Assert.AreEqual(null, result);
        }

        [Test]
        public void Test_CannotFindUserWithIncorrectPassword()
        {
            User result;
            result = authenticator.Authenticate("Test User C", "nopass");
            Assert.AreEqual(null, result);
        }

        [Test]
        public void Test_FindUserByCredentials()
        {
            User result;
            result = authenticator.Authenticate("Test User C", "password");
            Assert.AreEqual(users[2], result);
        }

        [Test]
        public void Test_FindUserByCredentialsNullPassword()
        {
            User result;
            result = authenticator.Authenticate("Test User E", null);
            Assert.AreEqual(users[4], result);
        }
    }
}

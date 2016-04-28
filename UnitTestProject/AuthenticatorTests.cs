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
    public class AuthenticatorTests
    {
        [Test]
        public void Test_InvalidUsername()
        {
            //Act
            User loggedInUser = authenticator.Authenticate("INVALID", "password");

            //Assert
            Assert.Null(loggedInUser);
        }

        [Test]
        public void Test_InvalidPassword()
        {
            //Act
            User loggedInUser = authenticator.Authenticate("Test User", "INVALID");

            //Assert
            Assert.Null(loggedInUser);
        }

        [Test]
        public void Test_EmptyCredentials()
        {
            //Act
            User loggedInUser = authenticator.Authenticate("", "");

            //Assert
            Assert.Null(loggedInUser);
        }

        [Test]
        public void Test_CorrectCredentials()
        {
            //Act
            User loggedInUser = authenticator.Authenticate("Test User", "password");

            //Assert
            Assert.AreEqual(users[0], loggedInUser);
        }

        private List<User> users;
        private Authenticator authenticator;

        [SetUp]
        public void Test_Initialize()
        {
            users = new List<User>();
            users.Add(createTestUser("Test User", "password", 99.99));
            authenticator = new Authenticator(users);
        }

        private User createTestUser(string name, string password, double balance)
        {
            User testUser = new User();
            testUser.Name = name;
            testUser.Password = password;
            testUser.Balance = balance;

            return testUser;
        }

        [TearDown]
        public void Test_Cleanup()
        {

        }
    }
}

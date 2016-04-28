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
        [SetUp]
        public void Test_Initialize()
        {

        }

        private User CreateTestUser(string userName, string password)
        {
            return CommonTestSetup.createTestUser(userName, password, 99.99);
        }
        
        [Test]
        public void Test_AuthenticateRightUsernameAndPasswordReturnsUser()
        {
            //Arrange
            const string USERNAME = "Test User";
            const string PASSWORD = "password";

            var users = new List<User>();
            User user = CreateTestUser(USERNAME, PASSWORD);
            users.Add(user);

            Authenticator authenticator = new Authenticator(users);

            //Act
            User returnedUser = authenticator.Authenticate(USERNAME, PASSWORD);

            //Assert
            Assert.AreEqual(user, returnedUser);
        }

        [Test]
        public void Test_AuthenticateRightUsernameAndNullPasswordReturnsUser()
        {
            //Arrange
            const string USERNAME = "Test User";

            var users = new List<User>();
            User user = CreateTestUser(USERNAME, "password");
            users.Add(user);

            Authenticator authenticator = new Authenticator(users);

            //Act
            User returnedUser = authenticator.Authenticate(USERNAME, null);

            //Assert
            Assert.AreEqual(user, returnedUser);
        }

        [Test]
        public void Test_AuthenticateRightUsernameAndWrongPasswordReturnsNull()
        {
            //Arrange
            const string USERNAME = "Test User";

            var users = new List<User>();
            users.Add(CreateTestUser(USERNAME, "password"));
            
            Authenticator authenticator = new Authenticator(users);

            //Act
            User returnedUser = authenticator.Authenticate(USERNAME, "WrongPassword");

            //Assert
            Assert.IsNull(returnedUser);
        }

        [Test]
        public void Test_AuthenticateWrongUsernameReturnsNull()
        {
            //Arrange
            var users = new List<User>();
            users.Add(CreateTestUser("Test User", "password"));

            Authenticator authenticator = new Authenticator(users);

            //Act
            User returnedUser = authenticator.Authenticate("Wrong User", "WrongPassword");

            //Assert
            Assert.IsNull(returnedUser);
        }

        [Test]
        public void Test_AuthenticateInvalidUsernameReturnsNull()
        {
            //Arrange
            var users = new List<User>();
            users.Add(CreateTestUser("Test User", "password"));

            Authenticator authenticator = new Authenticator(users);

            //Act
            User returnedUser = authenticator.Authenticate("", "WrongPassword");

            //Assert
            Assert.IsNull(returnedUser);
        }

        [TearDown]
        public void Test_Cleanup()
        {

        }
    }
}

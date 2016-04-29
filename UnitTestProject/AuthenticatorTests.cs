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
        private List<User> users;

        [TestFixtureSetUp]
        public void Class_Initialize()
        {
            users = new List<User>();
            users.Add(createTestUser("Test1", "", 10));
            users.Add(createTestUser("Test2", "t2", 20));
        }

        private User createTestUser(string name, string password, double balance)
        {
            User testUser = new User();
            testUser.Name = name;
            testUser.Password = password;
            testUser.Balance = balance;

            return testUser;
        }

        [Test]
        public void Test_InitializeWithoutError()
        {
            //Arrange
            //Act
            Authenticator authenticator = new Authenticator(users);

            //Assert
            Assert.Pass("No assertion really necessary here");
        }

        [Test]
        public void Test_AuthenticateUserSuccessfully()
        {
            //Arrange
            Authenticator authenticator = new Authenticator(users);

            //Act
            User user = authenticator.Authenticate("Test1", "");

            //Assert
            Assert.IsTrue(user.Name == "Test1");
        }

        [Test]
        public void Test_AuthenticationFailedIfUsernameIncorrect()
        {
            //Arrange
            Authenticator authenticator = new Authenticator(users);

            //Act
            User user = authenticator.Authenticate("Test3", "");

            //Assert
            Assert.IsTrue(user == null);
        }

        [Test]
        public void Test_AuthenticateUserSuccessfullyWithCorrectPassword()
        {
            //Arrange
            Authenticator authenticator = new Authenticator(users);

            //Act
            User user = authenticator.Authenticate("Test2", "t2");

            //Assert
            Assert.IsTrue(user.Name == "Test2");
        }

        [Test]
        public void Test_AuthenticationFailedIfPasswordIncorrect()
        {
            //Arrange
            Authenticator authenticator = new Authenticator(users);

            //Act
            User user = authenticator.Authenticate("Test2", "test22");

            //Assert
            Assert.IsTrue(user == null);
        }

        [Test]
        public void Test_AuthenticationFailedIfUsernameNull()
        {
            //Arrange
            Authenticator authenticator = new Authenticator(users);

            //Act
            User user = authenticator.Authenticate(null, "");

            //Assert
            Assert.IsTrue(user == null);
        }
        
    }
}

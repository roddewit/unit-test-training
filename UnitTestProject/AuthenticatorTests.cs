using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;
using NUnit.Framework;
using Refactoring;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace UnitTestProject
{
    [TestFixture]
    class AuthenticatorTests
    {
        private User createTestUser(string name, string password, double balance)
        {
            var testUser = new User {Name = name, Password = password, Balance = balance};

            return testUser;
        }

        [Test]
        public void Test_ShouldAuthenticateUser()
        {
            //Arrange
            var users = new List<User> { createTestUser("Unit Test", "", 100.00) };
            var authenticate = new Authenticator(users);

            //Act
            var user = authenticate.Authenticate(users.ToList().Select(r => r.Name).First(),
                users.ToList().Select(r => r.Password).First());

            //Assert
            Assert.IsNotNull(user);
        }

        [Test]
        public void Test_ShouldReturnNull()
        {
            //Arrange
            var users = new List<User> { createTestUser("Unit Test", "", 100.00) };
            var authenticate = new Authenticator(users);

            //Act
            var user = authenticate.Authenticate("",
                users.ToList().Select(r => r.Password).First());

            //Assert
            Assert.IsNull(user);
        }    
    }
}

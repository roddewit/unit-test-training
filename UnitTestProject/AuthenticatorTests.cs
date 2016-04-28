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
        public void Test_ShouldReturnAuthenticateUser()
        {
           //Arrange
            var users = new List<User>();
            users.Add(createTestUser("Test User", "", 99.99));
            Authenticator authenticate = new Authenticator(users);
            
            //Act
            var authenticatedUser=authenticate.Authenticate(users[0].Name, users[0].Password);
            Assert.IsNotNull(authenticatedUser);
            
           


        }

        [Test]
        public void Test_ShouldReturnNull()
        {
            //Arrange
            var users = new List<User>();
            users.Add(createTestUser("Test User", "", 99.99));
            Authenticator authenticate = new Authenticator(users);
            
            //Act
            var nullUser = authenticate.Authenticate("", users[0].Password);
            Assert.IsNull(nullUser);
        }

    }
}

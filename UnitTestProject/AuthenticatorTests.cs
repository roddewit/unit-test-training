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
        const string TEST_PRODUCT_ID = "1";
        private User createTestUser(string name, string password, double balance)
        {
            User testUser = new User();
            testUser.Name = name;
            testUser.Password = password;
            testUser.Balance = balance;

            return testUser;
        }

        [Test]
        public void Test_ValidUser()
        {


            var users = new List<User>();
            users.Add(createTestUser("Test User", "Test", 99.99));

            Authenticator authenticator = new Authenticator(users);

            //Act
            var user = authenticator.Authenticate("Test User", "Test");

            //Assert

            Assert.IsNotNull(user);
        }

        [Test]
        public void Test_UserNameInvalid()
        {
            

            var users = new List<User>();
            users.Add(createTestUser("Test User", "Test", 99.99));

             Authenticator authenticator = new Authenticator(users);

            //Act
             var user = authenticator.Authenticate("Invalid", "Test");
            
            //Assert
            Assert.IsNull(user);
            
        }

        [Test]
        public void Test_UserPasswordInvalid()
        {


            var users = new List<User>();
            users.Add(createTestUser("Test User", "Test", 99.99));

            Authenticator authenticator = new Authenticator(users);

            //Act
            var user = authenticator.Authenticate("Test User", "Invalid");

            //Assert
            Assert.IsNull(user);
           
        }
    }
}

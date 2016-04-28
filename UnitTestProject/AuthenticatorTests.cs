using Newtonsoft.Json;
using NUnit.Framework;
using Refactoring;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace UnitTestProject
{
    [TestFixture]
    class AuthenticatorTests
    {
        private User createTestUser(string name, string password)
        {
            User testUser = new User();
            testUser.Name = name;
            testUser.Password = password;

            return testUser;
        }
        
        [Test]
        public void Test_AuthenticUser()
        {
            //Arrange
            var testUser = new List<User>();
            
            testUser.Add(createTestUser("Frank", "pizza"));            
            var authenticate = new Authenticator( testUser );
            
            //Act 
            var authenticUser = authenticate.Authenticate("Frank", "pizza"); 

            //Assert
            Assert.AreEqual(testUser[0], authenticUser);
            
        }

        [Test]
        public void Test_NotValidUser()
        {
            //Arrange
            var testUser = new List<User>();

            testUser.Add(createTestUser("Bill", "pizza"));
            var authenticate = new Authenticator(testUser);

            //Act 
            var authenticUser = authenticate.Authenticate("", "pizza");

            //Assert
            Assert.AreNotEqual(testUser[0], authenticUser);

        }
        
    }
}

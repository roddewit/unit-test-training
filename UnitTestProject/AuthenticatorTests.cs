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
        private User createTestUser(string name, string password)
        {
            User testUser = new User();
            testUser.Name = name;
            testUser.Password = password;
            return testUser;
        }

        [Test]
        public void Test_Authenticate()
        {
            //Arrange
            var users = new List<User>();
            users.Add(createTestUser("UnitTest", "UnitTest"));

            var authenticate = new Authenticator(users);
            //Act
            authenticate.Authenticate("UnitTest", string.Empty);
            //Assert
            Assert.IsFalse(users[0].Password ==string.Empty);
        }
    }
}

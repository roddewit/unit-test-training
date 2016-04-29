using NUnit.Framework;
using Refactoring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject
{
    [TestFixture]
    class AuthenticatorTests
    {
        private Authenticator _auth;

        private User createTestUser(string name, string password)
        {
            User testUser = new User();
            testUser.Name = name;
            testUser.Password = password;

            return testUser;
        }

        private Authenticator createTestAuthenticator(params User[] users)
        {
            List<User> usersList = users.ToList();
            return new Authenticator(usersList);
        }

        [Test]
        public void Test_AuthenticateReturnsUserWhenValid()
        {
            //Arrange
            var testUser = createTestUser("Tester", "123");
            _auth = createTestAuthenticator(testUser);
            
            //Act
            var result = _auth.Authenticate(testUser.Name, testUser.Password);

            //Assert
            Assert.AreSame(testUser, result);

        }

        [Test]
        public void Test_AuthenticateReturnsNullForInvalidUserName()
        {
            //Arrange
            _auth = createTestAuthenticator(createTestUser("Tester", "123"));

            //Act
            var result = _auth.Authenticate(null, "123");

            //Assert
            Assert.Null(result);
        }


        [Test]
        public void Test_AuthenticateReturnsNullForInvalidPassword()
        {
            //Arrange
            var testUser = createTestUser("Tester", "123");
            _auth = createTestAuthenticator(testUser);

            //Act
            var result = _auth.Authenticate(testUser.Name, "Invalid Pass");

            //Assert
            Assert.Null(result);
        }

    }
}

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
        public void Test_AuthenticateReturnsValidUser_SingleUserExists()
        {
            //Arrange
            const string TEST_USER = "Test User A";
            const string TEST_USER_PASSWORD = "password";

            var users = new List<User>();
            users.Add(createTestUser(TEST_USER, TEST_USER_PASSWORD, 10.00));

            var authenticate = new Authenticator(users);

            //Act
            var validUser = authenticate.Authenticate(TEST_USER, TEST_USER_PASSWORD);

            //Assert
            Assert.IsTrue(validUser.Name.Equals(TEST_USER) && validUser.Password.Equals(TEST_USER_PASSWORD));
        }

        [Test]
        public void Test_AuthenticateReturnsValidUser_MultipleUsersExist()
        {
            //Arrange
            const string TEST_USER_A = "Test User A";
            const string TEST_USER_B = "Test User B";
            const string TEST_USER_A_PASSWORD = "thisismypassword";
            const string TEST_USER_B_PASSWORD = "12345";

            var users = new List<User>();
            users.Add(createTestUser(TEST_USER_A, TEST_USER_A_PASSWORD, 10.00));
            users.Add(createTestUser(TEST_USER_B, TEST_USER_B_PASSWORD, 10.00));

            var authenticate = new Authenticator(users);

            //Act
            var validUser = authenticate.Authenticate(TEST_USER_B, TEST_USER_B_PASSWORD);

            //Assert
            Assert.IsTrue(validUser.Name.Equals(TEST_USER_B) && validUser.Password.Equals(TEST_USER_B_PASSWORD));
        }
    
        [Test]
        public void Test_AuthenticateInvalidCredentialsReturnsNoUser()
        {
            //Arrange
            const string TEST_USER = "Test User A";
            const string TEST_USER_PASSWORD = "password";

            var users = new List<User>();
            users.Add(createTestUser(TEST_USER, TEST_USER_PASSWORD, 10.00));

            var authenticate = new Authenticator(users);

            //Act
            var validUser = authenticate.Authenticate("Bob", "password");

            //Assert
            Assert.IsNull(validUser);
        }
    
        [Test]
        public void Test_AuthenticateNullUsernameReturnsNoUser()
        {
            //Arrange
            const string TEST_USER = "Test User A";
            const string TEST_USER_PASSWORD = "password";

            var users = new List<User>();
            users.Add(createTestUser(TEST_USER, TEST_USER_PASSWORD, 10.00));

            var authenticate = new Authenticator(users);

            //Act
            var validUser = authenticate.Authenticate(null, "password");

            //Assert
            Assert.IsNull(validUser);
        }

        [Test]
        public void Test_AuthenticateNullPasswordReturnsNoUser()
        {
            //Arrange
            const string TEST_USER = "Test User A";
            const string TEST_USER_PASSWORD = "password";

            var users = new List<User>();
            users.Add(createTestUser(TEST_USER, TEST_USER_PASSWORD, 10.00));

            var authenticate = new Authenticator(users);

            //Act
            var validUser = authenticate.Authenticate("Test User A", null);

            //Assert
            Assert.IsNull(validUser);
        }
    
        [Test]
        public void Test_AuthenticateWithNullUsersListReturnsNoUser()
        {
            //Arrange
            const string TEST_USER = "Bob";
            const string TEST_USER_PASSWORD = "12345";

            var authenticate = new Authenticator(null);

            //Act
            var validUser = authenticate.Authenticate(TEST_USER, TEST_USER_PASSWORD);

            //Assert
            Assert.IsNull(validUser);
        }
    
        [Test]
        public void Test_AuthenticateBlankUsernameReturnsNoUser()
        {
            //Arrange
            const string TEST_USER = "Test User A";
            const string TEST_USER_PASSWORD = "password";

            var users = new List<User>();
            users.Add(createTestUser(TEST_USER, TEST_USER_PASSWORD, 10.00));

            var authenticate = new Authenticator(users);

            //Act
            var validUser = authenticate.Authenticate("", "password");

            //Assert
            Assert.IsNull(validUser);
        }
    
        [Test]
        public void Test_AuthenticateBlankPasswordReturnsNoUser()
        {
            //Arrange
            const string TEST_USER = "Test User A";
            const string TEST_USER_PASSWORD = "password";

            var users = new List<User>();
            users.Add(createTestUser(TEST_USER, TEST_USER_PASSWORD, 10.00));

            var authenticate = new Authenticator(users);

            //Act
            var validUser = authenticate.Authenticate("Test User A", "");

            //Assert
            Assert.IsNull(validUser);
        }
    }
}

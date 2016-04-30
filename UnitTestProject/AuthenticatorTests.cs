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
        private User expectedUser = new User();

        private User createTestUser(string name, string password, double balance) {
            User testUser = new User();
            testUser.Name = name;
            testUser.Password = password;
            testUser.Balance = balance;

            return testUser;
        }

        private Authenticator createAuthenticator(bool validUser)
        {
            expectedUser = null;
            if (validUser) { 
                List<User> users = new List<User>();
                expectedUser = createTestUser("Test", "Test", 99.99);
                users.Add(expectedUser);

                return new Authenticator(users);
            }
            return new Authenticator(null);
        }

        [Test]
        public void Test_AuthenticateValidUsernameAndPassword()
        {
            Authenticator authenticator = createAuthenticator(true);
            User actualUser = authenticator.Authenticate("Test", "Test");

            Assert.IsNotNull(actualUser);
            Assert.AreEqual(expectedUser.Name, actualUser.Name);
            Assert.AreEqual(expectedUser.Password, actualUser.Password);
            Assert.AreEqual(expectedUser.Balance, actualUser.Balance);
        }

        [Test]
        public void Test_AuthenticateInvalidUsernameAndInvalidPassword()
        {
            Authenticator authenticator = createAuthenticator(true);
            User actualUser = authenticator.Authenticate("asdf", "asdf");

            Assert.IsNull(actualUser);
        }

        [Test]
        public void Test_AuthenticateInvalidUsernameAndValidPassword()
        {
            Authenticator authenticator = createAuthenticator(true);
            User actualUser = authenticator.Authenticate("asdf", "Test");

            Assert.IsNull(actualUser);
        }

        [Test]
        public void Test_AuthenticateValidUsernameAndInvalidPassword()
        {
            Authenticator authenticator = createAuthenticator(true);
            User actualUser = authenticator.Authenticate("Test", "adsdf");

            Assert.IsNull(actualUser);
        }

        [Test]
        public void Test_AuthenticateNullUsernameAndNullPassword()
        {
            Authenticator authenticator = createAuthenticator(true);
            User actualUser = authenticator.Authenticate(null, null);

            Assert.IsNull(actualUser);
        }

        [Test]
        public void Test_AuthenticateNullUsernameAndValidPassword()
        {
            Authenticator authenticator = createAuthenticator(true);
            User actualUser = authenticator.Authenticate(null, "Test");

            Assert.IsNull(actualUser);
        }

        [Test]
        public void Test_AuthenticateNullUsernameAndInvalidPassword()
        {
            Authenticator authenticator = createAuthenticator(true);
            User actualUser = authenticator.Authenticate(null, "asd");

            Assert.IsNull(actualUser);
        }

        [Test]
        public void Test_AuthenticateValidUsernameAndNullPassword()
        {
            Authenticator authenticator = createAuthenticator(true);
            User actualUser = authenticator.Authenticate("Test", null);

            Assert.IsNull(actualUser);
        }

        [Test]
        public void Test_AuthenticateInvalidUsernameAndNullPassword()
        {
            Authenticator authenticator = createAuthenticator(true);
            User actualUser = authenticator.Authenticate("asdf", null);

            Assert.IsNull(actualUser);
        }

        [Test]
        public void Test_AuthenticateEmptyUsernameAndEmptyPassword()
        {
            Authenticator authenticator = createAuthenticator(true);
            User actualUser = authenticator.Authenticate("", "");

            Assert.IsNull(actualUser);
        }

        [Test]
        public void Test_AuthenticateEmptyUsernameAndValidPassword()
        {
            Authenticator authenticator = createAuthenticator(true);
            User actualUser = authenticator.Authenticate("", "Test");

            Assert.IsNull(actualUser);
        }

        [Test]
        public void Test_AuthenticateEmptyUsernameAndInvalidPassword()
        {
            Authenticator authenticator = createAuthenticator(true);
            User actualUser = authenticator.Authenticate("", "asd");

            Assert.IsNull(actualUser);
        }

        [Test]
        public void Test_AuthenticateValidUsernameAndEmptyPassword()
        {
            Authenticator authenticator = createAuthenticator(true);
            User actualUser = authenticator.Authenticate("Test", "");

            Assert.IsNull(actualUser);
        }

        [Test]
        public void Test_AuthenticateInvalidUsernameAndEmptyPassword()
        {
            Authenticator authenticator = createAuthenticator(true);
            User actualUser = authenticator.Authenticate("asdf", "");

            Assert.IsNull(actualUser);
        }

        [Test]
        public void Test_AuthenticateEmptyUsernameAndNullPassword()
        {
            Authenticator authenticator = createAuthenticator(true);
            User actualUser = authenticator.Authenticate("", null);

            Assert.IsNull(actualUser);
        }

        [Test]
        public void Test_AuthenticatNullUsernameAndEmptyPassword()
        {
            Authenticator authenticator = createAuthenticator(true);
            User actualUser = authenticator.Authenticate("", null);

            Assert.IsNull(actualUser);
        }

        [Test]
        public void Test_AuthenticateReturnNullIfCaseIsNotCorrectForPassword()
        {
            Authenticator authenticator = createAuthenticator(true);
            User actualUser = authenticator.Authenticate("Test", "test");

            Assert.IsNull(actualUser);
        }

        [Test]
        public void Test_AuthenticateReturnNullIfCaseIsNotCorrectForUsername()
        {
            Authenticator authenticator = createAuthenticator(true);
            User actualUser = authenticator.Authenticate("test", "Test");

            Assert.IsNull(actualUser);
        }

        [Test]
        public void Test_AuthenticateReturnNullIfListOfUserIsNullPassedIn()
        {
            Authenticator authenticator = createAuthenticator(false);
            User actualUser = authenticator.Authenticate("test", "Test");

            Assert.IsNull(actualUser);
        }
    }
}

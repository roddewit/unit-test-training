using System.Collections.Generic;
using NUnit.Framework;
using Refactoring;

namespace UnitTestProject
{
    [TestFixture]
    public class AuthenticatorTests
    {
        private List<User> createUsersList()
        {
            var users = new List<User>
            {
                new User() {Balance = 10.00, Name = "Harry", Password = "password"} ,
                new User(){Balance = 10.00, Name = "Jane", Password = "pwd"} ,
                new User() {Balance = 10.00, Name = "Peter", Password = ""} ,
                new User() {Balance = 10.00, Name = "George", Password = "test"} ,
                new User() {Balance = 10.00, Name = "Teresa", Password = null} ,
            };

            return users;
        }

        [Test]
        public void Test_AuthenticateEmptyUsername()
        {
            var users = createUsersList();
            var authenticator = new Authenticator(users);

            var user = authenticator.Authenticate("", "password");

            Assert.Null(user);
        }

        [Test]
        public void Test_AuthenticateNullUsername()
        {
            var users = createUsersList();
            var authenticator = new Authenticator(users);

            var user = authenticator.Authenticate(null, "password");

            Assert.Null(user);
        }

        [Test]
        public void Test_AuthenticateUserNotFound()
        {
            var users = createUsersList();
            var authenticator = new Authenticator(users);

            var user = authenticator.Authenticate("Sara", "password");

            Assert.Null(user);
        }

        [Test]
        public void Test_AuthenticateInvalidPassword()
        {
            var users = createUsersList();
            var authenticator = new Authenticator(users);

            var user = authenticator.Authenticate("Jane", "password");

            Assert.Null(user);
        }

        [Test]
        public void Test_AuthenticateEmptyStringPassword()
        {
            var users = createUsersList();
            var authenticator = new Authenticator(users);

            var user = authenticator.Authenticate("Peter", "");

            if (user != null)
            {
                Assert.AreEqual("Peter", user.Name);
                Assert.AreEqual("", user.Password);
                Assert.AreEqual(10.00, user.Balance);
            }
            else
            {
                Assert.Fail("The user returned was null");
            }
        }

        [Test]
        public void Test_AuthenticateNullPassword()
        {
            var users = createUsersList();
            var authenticator = new Authenticator(users);

            var user = authenticator.Authenticate("Teresa", null);

            if (user != null)
            {
                Assert.AreEqual("Teresa", user.Name);
                Assert.AreEqual(null, user.Password);
                Assert.AreEqual(10.00, user.Balance);
            }
            else
            {
                Assert.Fail("The user returned was null");
            }
        }

        [Test]
        public void Test_AuthenticateUsernameAndPasswordFound()
        {
            var users = createUsersList();
            var authenticator = new Authenticator(users);

            var user = authenticator.Authenticate("Jane", "pwd");

            if (user != null)
            {
                Assert.AreEqual("Jane", user.Name);
                Assert.AreEqual("pwd", user.Password);
                Assert.AreEqual(10.00, user.Balance);
            }
            else
            {
                Assert.Fail("The user returned was null");
            }
        }
    }
}

using Newtonsoft.Json;
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
        private User testUser;
        private Authenticator authenticator;

        [Test]
        public void Test_Authenticate_ShouldSucceed()
        {
            User user = authenticator.Authenticate("Chris", "");
            Assert.AreEqual(testUser, user);
        }

        [Test]
        public void Test_Authenticate_InvalidPassword()
        {
            User user = authenticator.Authenticate("Chris", "asdf");
            Assert.Null(user);
        }

        [Test]
        public void Test_Authenticate_NullUserName()
        {
            User user = authenticator.Authenticate(null, "");
            Assert.Null(user);
        }

        [SetUp]
        public void Test_Initialize()
        {
            List<User> users = new List<User>();
            testUser = new User();
            testUser.Name = "Chris";
            testUser.Password = "";
            testUser.Balance = 100;
            users.Add(testUser);

            authenticator = new Authenticator(users);
        }
    }
}

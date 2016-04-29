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
        private Authenticator Authenticator;

        [SetUp]
        public void Test_Initialize()
        {
            List<User> users = new List<User>(5);
            users.Add(new User() { Name = "User1", Password = "PW1", Balance = 0.00 });
            users.Add(new User() { Name = "User2", Password = "PW2", Balance = 5.00 });
            users.Add(new User() { Name = "User3", Password = "PW3", Balance = 10.00 });
            users.Add(new User() { Name = "User4", Password = "PW4", Balance = 20.00 });
            users.Add(new User() { Name = "User5", Password = "PW5", Balance = 30.00 });

            Authenticator = new Authenticator(users);
        }

        [Test]
        public void AuthenticateNullUser()
        {
            Assert.IsNull(Authenticator.Authenticate(null, ""));
        }

        [Test]
        public void AuthenticateNullPassword()
        {
            Assert.IsNull(Authenticator.Authenticate("", null));
        }

        [Test]
        public void AuthenticateBadPassword()
        {
            Assert.IsNull(Authenticator.Authenticate("User1", ""));
        }

        [Test]
        public void AuthenticateUserDoesntExist()
        {
            Assert.IsNull(Authenticator.Authenticate("User6", "PW6"));
        }

        [Test]
        public void AuthenticateFirstUser()
        {
            var user = Authenticator.Authenticate("User1", "PW1");
            Assert.IsNotNull(user);
            Assert.AreEqual("User1", user.Name);
        }

        [Test]
        public void AuthenticateLastUser()
        {
            var user = Authenticator.Authenticate("User5", "PW5");
            Assert.IsNotNull(user);
            Assert.AreEqual("User5", user.Name);
        }

        [Test]
        public void AuthenticateMiddleUser()
        {
            var user = Authenticator.Authenticate("User3", "PW3");
            Assert.IsNotNull(user);
            Assert.AreEqual("User3", user.Name);
        }
    }
}

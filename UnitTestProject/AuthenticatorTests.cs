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
    public class AuthenticatorTests
    {
        Authenticator authenticator;

        [SetUp]
        public void Test_Initialize()
        {
            var users = new List<User>();

            var user = new User();
            user.Name = "test";
            user.Password = "password";

            users.Add(user);

            authenticator = new Authenticator(users);            
        }

        [Test]
        public void TestAuthenticate()
        {
            var user = authenticator.Authenticate("test", "password");

            Assert.IsNotNull(user);
        }
    }
}

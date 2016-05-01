using System.IO;
using System.Collections.Generic;
using Refactoring;
using NUnit.Framework;
using Newtonsoft.Json;

namespace UnitTestProject
{
    [TestFixture]
    class AuthenticatorTests
    {
        private Authenticator authenticator;
        private List<User> users;
        [SetUp]
        public void Test_Initialize ()
        {
            users = new List<User>();
            authenticator = new Authenticator( users );
        }

        [Test]
        [ExpectedException( typeof( NullUserListException ) )]
        public void Test_UserListCannotbeNull ()
        {
            new Authenticator( null );
            Assert.Fail();
        }


    }
}


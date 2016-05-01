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

        [TearDown]
        public void Test_CleanUp ()
        {
            users = null;
            authenticator =null;
        }

        [Test]
        [ExpectedException( typeof( NullUserListException ) )]
        public void Test_UserListCannotbeNull ()
        {
            new Authenticator( null );
            Assert.Fail();
        }

        [Test]
        public void Test_UserCannotLoginIfNotExist ()
        {
            string password = "NOTEXIST";
            string name = "NOTEXIST";
            Assert.IsNull(authenticator.Authenticate( name, password ));
        }

        [Test]
        public void Test_UserCanLogin ()
        {
            string password = "TEST";
            string name = "TESTNAME";
            User user = new User();
            user.Password = password;
            user.Name = name;
            users.Add( user );
            User authenticated = authenticator.Authenticate( name, password );

            Assert.IsNotNull( authenticated );
            Assert.IsTrue( name == authenticated.Name );
            Assert.IsTrue( password == authenticated.Password );
        }

        [Test]
        public void Test_UserCanNotLoginIfPasswordIsNotCorrect ()
        {
            string password = "TEST";
            string name = "TESTNAME";
            User user = new User();
            user.Password = password;
            user.Name = name;
            users.Add( user );
            User authenticated = authenticator.Authenticate( name, "wrongPass" );

            Assert.Null( authenticated );
        }

        [Test]
        public void Test_UserCanNotLoginIfPasswordIsNull ()
        {
            string password = "TEST";
            string name = "TESTNAME";
            User user = new User();
            user.Password = password;
            user.Name = name;
            users.Add( user );
            User authenticated = authenticator.Authenticate( name, null );

            Assert.IsNull( authenticated );
        }

        [Test]
        public void Test_IncorrectUserNameCanNotLogin ()
        {
            string password = "TEST";
            string name = "TESTNAME";
            User user = new User();
            user.Password = password;
            user.Name = name;
            users.Add( user );
            User authenticated = authenticator.Authenticate( "NotAName", password );

            Assert.IsNull( authenticated );
        }


    }
}


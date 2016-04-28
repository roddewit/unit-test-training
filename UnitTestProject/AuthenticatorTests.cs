using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Refactoring;

namespace UnitTestProject
{
    [TestFixture]
    class AuthenticatorTests
    {
        const string USERNAME = "TEST";
        const string PWD = "PWD";
        List<User> users;
        Authenticator auth;
       
        [SetUp]
        public void Common()
        {
            users = new List<User>();
            users.Add(createTestUser(USERNAME, PWD, 0));
            auth = new Authenticator(users);
        }

        private User createTestUser(string name, string password, double balance)
        {
            User testUser = new User();
            testUser.Name = name;
            testUser.Password = password;
            testUser.Balance = balance;

            return testUser;
        }

        [Test]
        public void AuthenticateUserSuccess()
        {
            //Assert
            Assert.IsNotNull(auth.Authenticate(USERNAME,PWD));
        }

        [Test]
        public void AuthenticateUserFailForPwd()
        {
            //Assert
            Assert.IsNull(auth.Authenticate(USERNAME, PWD + "1"));
        }
        [Test]
        public void AuthenticateUserFailForusername()
        {
            //Assert
            Assert.IsNull(auth.Authenticate(USERNAME + "1", PWD));
        }

        [Test]
        public void AuthenticateUserFailForBoth()
        {
            //Assert
            Assert.IsNull(auth.Authenticate(USERNAME + "1", PWD + "1"));
        }
    }
}

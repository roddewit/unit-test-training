using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using NUnit.Framework;
using Refactoring;

namespace UnitTestProject
{
    [TestFixture]
    public class AuthenticatorTests
    {
        private Authenticator _AUTHENTICATOR;
        private List<User> _USERS;

        [Test]
        public void Test_Authenticate_EmptyUserName()
        {
            //Act
            User actual = _AUTHENTICATOR.Authenticate("", "");

            //Assert
            Assert.IsNull(actual);
        }

        [Test]
        public void Test_Authenticate_NullUserName()
        {
            //Act
            User actual = _AUTHENTICATOR.Authenticate(null, null);

            //Assert
            Assert.IsNull(actual);
        }

        [Test]
        public void Test_Authenticate_InvalidUserName()
        {
            //Act
            User actual = _AUTHENTICATOR.Authenticate("ThisIsNotAValidUser", "ThisIsNotAValidPassword");

            //Assert
            Assert.IsNull(actual);
        }

        [Test]
        public void Test_Authenticate_InvalidPassword()
        {
            //Act
            User actual = _AUTHENTICATOR.Authenticate(_USERS[0].Name, "ThisIsNotAValidPassword");

            //Assert
            Assert.IsNull(actual);
        }

        [Test]
        public void Test_Authenticate_ValidUser()
        {
            //Arrange
            User expected = _USERS[0];

            //Act
            User actual = _AUTHENTICATOR.Authenticate(_USERS[0].Name, _USERS[0].Password);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Test_Authenticate_AnotherUsersPassword()
        {
            User actual = _AUTHENTICATOR.Authenticate(_USERS[0].Name, _USERS[1].Password);

            //Assert
            Assert.IsNull(actual);
        }

        [Test]
        public void Test_Authenticate_LastUser()
        {
            //Arrange
            User expected = _USERS[_USERS.Count - 1];

            User actual = _AUTHENTICATOR.Authenticate(_USERS[_USERS.Count - 1].Name, _USERS[_USERS.Count - 1].Password);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Test_Authenticate_TwoUsersWithSameName_FirstUser()
        {
            //Arrange
            List<User> users = new List<User>();

            User userA1 = new User();
            userA1.Balance = 1000.00;
            userA1.Name = "UserA";
            userA1.Password = "PassA";

            User userA2 = new User();
            userA2.Balance = 1.00;
            userA2.Name = "UserA";
            userA2.Password = "PassB";

            users.Add(userA1);
            users.Add(userA2);

            Authenticator authenicator = new Authenticator(users);

            //Act
            User actual = authenicator.Authenticate(userA1.Name, userA1.Password);

            //Assert
            Assert.AreSame(userA1, actual);
        }

        [Test]
        public void Test_Authenticate_TwoUsersWithSameName_SecondUser()
        {
            //Arrange
            List<User> users = new List<User>();

            User userA1 = new User();
            userA1.Balance = 1000.00;
            userA1.Name = "UserA";
            userA1.Password = "PassA";

            User userA2 = new User();
            userA2.Balance = 1.00;
            userA2.Name = "UserA";
            userA2.Password = "PassB";

            users.Add(userA1);
            users.Add(userA2);

            Authenticator authenicator = new Authenticator(users);

            //Act
            User actual = authenicator.Authenticate(userA2.Name, userA2.Password);

            //Assert
            Assert.AreSame(userA2, actual);
        }

        [Test]
        public void Test_Authenticate_TwoUsersWithSameNameAndPassword()
        {
            //Arrange
            List<User> users = new List<User>();

            User userA1 = new User();
            userA1.Balance = 1000.00;
            userA1.Name = "UserA";
            userA1.Password = "PassA";

            User userA2 = new User();
            userA2.Balance = 1.00;
            userA2.Name = "UserA";
            userA2.Password = "PassA";

            users.Add(userA1);
            users.Add(userA2);

            Authenticator authenicator = new Authenticator(users);

            //Act
            User actual = authenicator.Authenticate(userA2.Name, userA2.Password);

            //Assert
            Assert.IsNull(actual, "It should not be possible to have two users with the same name and password.");
        }



        [SetUp]
        public void Test_Initialize()
        {
            _USERS = CreateUsers();
            _AUTHENTICATOR = new Authenticator(_USERS);
        }
        
        [TearDown]
        public void Test_Cleanup()
        {
            _USERS = null;
            _AUTHENTICATOR = null;
        }

        private List<User> CreateUsers()
        {
            List<User> users = new List<User>();

            User userA = new User();
            userA.Balance = 1.00;
            userA.Name = "UserA";
            userA.Password = "PassA";

            User userB = new User();
            userB.Balance = 10.00;
            userB.Name = "UserB";
            userB.Password = "PassB";

            User userC = new User();
            userC.Balance = 100.00;
            userC.Name = "UserC";
            userC.Password = "PassC";

            User userD = new User();
            userD.Balance = 0.00;
            userD.Name = "UserD";
            userD.Password = "PassD";

            User userE = new User();
            userE.Balance = -1.00;
            userE.Name = "UserE";
            userE.Password = "PassE";

            users.Add(userA);
            users.Add(userB);
            users.Add(userC);
            users.Add(userD);
            users.Add(userE);

            return users;
        }
    }
}

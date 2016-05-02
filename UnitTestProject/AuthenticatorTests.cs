using System;
using System.Collections.Generic;
using Refactoring;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject
{
    [TestClass]
    public class AuthenticatorTests
    {
        #region Variables
        private const string FirstUserName = "John";
        private const string FirstUserPassword = "";

        private const string EmptyUserName = "";
        private const string EmptyUserPassword = "";
        #endregion Variables

        #region TestSetUp
        private User CreateEmptyTestUser()
        {
            User testUser = new User();
            testUser.Name = EmptyUserName;
            testUser.Password = EmptyUserPassword;
            
            return testUser;
        }

        private User CreateFirstTestUser()
        {
            User testUser = new User();
            testUser.Name = FirstUserName;
            testUser.Password = FirstUserPassword;

            return testUser;
        }

        #endregion TestSetUp

        #region TestMethod
        [TestMethod] 
        public void Test_NullUserName()
        {
            //Assign
            var users = new List<User>();
            users.Add(CreateEmptyTestUser());    

            //Act
            var auth = new Authenticator(users);
            
            //Assert
            Assert.IsNull(auth.Authenticate(users[0].Name, users[0].Password));

        }

        [TestMethod] 
        public void Test_ContainUserByUserName()
        {
            //Assign
            var users = new List<User>();
            users.Add(CreateFirstTestUser());

            //Act
            var auth = new Authenticator(users);

            //Assert
            Assert.AreEqual(FirstUserName, users[0].Name);

        }
        #endregion TestMethod

    }
}

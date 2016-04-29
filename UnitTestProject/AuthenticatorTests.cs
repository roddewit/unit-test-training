using System;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json;
using NUnit.Framework;
using Refactoring;
using System.IO;

namespace UnitTestProject
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestFixture]
    public class AuthenticatorTests
    {
        const string TEST_USER_NAME = "Bob";
        const string TEST_USER_PASSWORD = "pass123";
        const string TEST_USER_INCORRECT_PASSWORD = "totallynotthepassword";
        const double ALL_TEST_USERS_BALANCE = 1.00;

        private User createTestUser(string name, string password)
        {
            User testUser = new User();
            testUser.Name = name;
            testUser.Password = password;
            testUser.Balance = ALL_TEST_USERS_BALANCE;

            return testUser;
        }

        private List<User> CreateTestUserList(params string[] userNamesAndPasswords)
        {
            var userNameAndPasswordListCountIsOdd = (userNamesAndPasswords.Length % 2 == 1);

            if (userNameAndPasswordListCountIsOdd)
            {
                throw new Exception("Test user list needs matching set of usernames and passwords.");
            }

            var userList = new List<User>();

            for (int i = 0; i < userNamesAndPasswords.Length; i = i + 2)
            {
                var userName = userNamesAndPasswords[i];
                var password = userNamesAndPasswords[i + 1];

                userList.Add(createTestUser(userName, password));
            }

            return userList;
        }

        private Authenticator CreateTestAuthenticator()
        {
            var userList = CreateTestUserList("Sarah", "12345", TEST_USER_NAME, TEST_USER_PASSWORD, "John", "supersecretpassword", "Dog", "dogpassword2");

            return new Authenticator(userList);
        }

        [Test]
        public void Test_AuthenticationWorksWithCorrectPassword()
        {
            var authenticator = CreateTestAuthenticator();

            var user = authenticator.Authenticate(TEST_USER_NAME, TEST_USER_PASSWORD);

            Assert.IsNotNull(user);
            Assert.AreEqual(TEST_USER_NAME, user.Name);
            Assert.AreEqual(TEST_USER_PASSWORD, user.Password);
        }

        [Test]
        public void Test_AuthenticationFailsWithIncorrectPassword()
        {
            var authenticator = CreateTestAuthenticator();

            var user = authenticator.Authenticate(TEST_USER_NAME, TEST_USER_INCORRECT_PASSWORD);

            Assert.IsNull(user);
        }

        [Test]
        public void Test_AuthenticationFailsWithBlankUserName()
        {
            var authenticator = CreateTestAuthenticator();

            var user = authenticator.Authenticate("", TEST_USER_PASSWORD);

            Assert.IsNull(user);
        }
    }
}

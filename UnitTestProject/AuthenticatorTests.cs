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
        private User createUser(String name, String password)
        {
            User result = new User();
            result.Name = name;
            result.Password = password;
            return result;
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Test_SearchingANullListThrows()
        {
            Authenticator authenticator = new Authenticator(null);

            User result = authenticator.Authenticate("Test", "Test");
        }

        [Test]
        public void Test_SearchingForAUserNotInTheListReturnsNull()
        {
            Authenticator authenticator = new Authenticator(new List<User>());

            User result = authenticator.Authenticate("Test", "Test");

            Assert.IsNull(result);
        }

        [Test]
        public void Test_SearchingForAUserWithMatchingNameAndPasswordReturnsTheRightUser()
        {
            List<User> users = new List<User>();
            User firstUser = createUser("Test", "Test");
            users.Add(firstUser);
            Authenticator authenticator = new Authenticator(users);

            User result = authenticator.Authenticate("Test", "Test");

            Assert.AreSame(firstUser, result);
        }

        [Test]
        public void Test_SearchingForAUserWithMatchingNameAndMismatchedPasswordReturnsNull()
        {
            List<User> users = new List<User>();
            User firstUser = createUser("Test", "Test");
            users.Add(firstUser);
            Authenticator authenticator = new Authenticator(users);

            User result = authenticator.Authenticate("Test", "Blah");

            Assert.IsNull(result);
        }

        [Test]
        public void Test_PasswordSearchingIsCaseSensitive()
        {
            List<User> users = new List<User>();
            User firstUser = createUser("Test", "Test");
            users.Add(firstUser);
            Authenticator authenticator = new Authenticator(users);

            User result = authenticator.Authenticate("Test", "TEST");

            Assert.IsNull(result);
        }

        [Test]
        public void Test_UsernameSearchingIsCaseSensitive()
        {
            List<User> users = new List<User>();
            User firstUser = createUser("Test", "Test");
            users.Add(firstUser);
            Authenticator authenticator = new Authenticator(users);

            User result = authenticator.Authenticate("TEST", "Test");

            Assert.IsNull(result);
        }

        [Test]
        public void Test_SearchingForAUserWithMatchingNameAndPasswordReturnsTheRightUserEvenIfOtherUsersWithSameNameExist()
        {
            List<User> users = new List<User>();
            User firstUser = createUser("Test", "Test");
            User secondUser = createUser("Test", "anotherPassword");
            users.Add(firstUser);
            users.Add(secondUser);
            Authenticator authenticator = new Authenticator(users);

            User result = authenticator.Authenticate("Test", "anotherPassword");

            Assert.AreSame(firstUser, authenticator.Authenticate("Test", "Test"));
            Assert.AreSame(secondUser, authenticator.Authenticate("Test", "anotherPassword"));
        }

        [Test]
        public void Test_SearchingWithNullUsernameReturnsNullEvenIfPasswordMatchesSomeone()
        {
            List<User> users = new List<User>();
            User firstUser = createUser("Test", "Test");
            users.Add(firstUser);
            Authenticator authenticator = new Authenticator(users);

            User result = authenticator.Authenticate(null, "Test");

            Assert.IsNull(result);
        }

        [Test]
        public void Test_SearchingWithNullPasswordReturnsUserWithMatchingName()
        {
            List<User> users = new List<User>();
            User firstUser = createUser("Test", "Test");
            users.Add(firstUser);
            Authenticator authenticator = new Authenticator(users);

            User result = authenticator.Authenticate("Test", null);

            Assert.AreSame(firstUser, result);
        }

        [Test]
        public void Test_SearchingWithNullPasswordReturnsFirstUserWithMatchingNameIfManyWithSameNameThere()
        {
            List<User> users = new List<User>();
            User firstUser = createUser("Test", "Test");
            User secondUser = createUser("Test", "Blah");
            users.Add(firstUser);
            users.Add(secondUser);
            Authenticator authenticator = new Authenticator(users);

            User result = authenticator.Authenticate("Test", null);

            Assert.AreSame(firstUser, result);
        }

        [Test]
        public void Test_SearchingWithNullPasswordReturnsFirstUserWithMatchingNameEvenWhenSomeUsersAreBeforeItInTheList()
        {
            List<User> users = new List<User>();
            User firstUser = createUser("Blah", "Test");
            User secondUser = createUser("Test", "Test");
            users.Add(firstUser);
            users.Add(secondUser);
            Authenticator authenticator = new Authenticator(users);

            User result = authenticator.Authenticate("Test", null);

            Assert.AreSame(secondUser, result);
        }
    }
}

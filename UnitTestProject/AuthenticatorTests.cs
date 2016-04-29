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
	public class AuthenticatorTests
	{
		private User createTestUser(string name, string password, double balance)
		{
			User testUser = new User();
			testUser.Name = name;
			testUser.Password = password;
			testUser.Balance = balance;

			return testUser;
		}

		private List<User> createTestUsers()
		{
			List<User> testUsers = new List<User>();

			for (int i = 0; i < 3; i++)
			{
				testUsers.Add(createTestUser("TestUser" + i, i.ToString(), 99.99));
			}
			
			return testUsers;
		}

		[Test]
		public void Test_GoodAuthenticate()
		{
			//Arrange
			var testUsers = createTestUsers();
			var authenticator = new Authenticator(testUsers);

			//Act
			var user = authenticator.Authenticate(testUsers[0].Name, "0");

			//Assert
			Assert.NotNull(user);
		}

		[Test]
		public void Test_BadAuthenticate()
		{
			//Arrange
			var testUsers = createTestUsers();
			var authenticator = new Authenticator(testUsers);
			
			//Act
			var user = authenticator.Authenticate(testUsers[0].Name, "password");

			//Assert
			Assert.Null(user);
		}

		[Test]
		public void Test_NoUsers()
		{
			//Arrange
			var testUsers = new List<User>();
			var authenticator = new Authenticator(testUsers);

			//Act
			var user = authenticator.Authenticate("", "");

			//Assert
			Assert.Null(user);
		}

		[Test]
		public void Test_BlankPassword()
		{
			//Arrange
			var testUsers = createTestUsers();
			var authenticator = new Authenticator(testUsers);

			//Act
			var user = authenticator.Authenticate(testUsers[0].Name, "");

			//Assert
			Assert.Null(user);
		}
	}
}

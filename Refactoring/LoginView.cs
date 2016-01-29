using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Refactoring
{
    public class LoginView
    {
        private readonly Authenticator authenticator;

        public LoginView(Authenticator authenticator)
        {
            this.authenticator = authenticator;
        }

        public User Login()
        {
            string username = GetUsername();
            if (String.IsNullOrEmpty(username))
            {
                throw new EmptyUsernameException();
            }

            string password = GetPassword();

            return authenticator.Authenticate(username, password);
        }

        private string GetUsername()
        {
            Console.WriteLine();
            Console.WriteLine("Enter Username:");
            return Console.ReadLine();
        }

        private string GetPassword()
        {
            Console.WriteLine("Enter Password:");
            return Console.ReadLine();
        }
    }
}

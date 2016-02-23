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
        private readonly IConsole console;

        public LoginView(IConsole console, Authenticator authenticator)
        {
            this.console = console;
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
            console.WriteLine();
            console.WriteLine("Enter Username:");
            return console.ReadLine();
        }

        private string GetPassword()
        {
            console.WriteLine("Enter Password:");
            return console.ReadLine();
        }
    }
}

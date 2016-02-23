using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Refactoring
{
    public interface IConsole
    {
        void WriteLine();
        void WriteLine(string text);

        string ReadLine();

        void Clear();
        ConsoleColor ForegroundColor { get; set; }
        void ResetColor();
    }
}

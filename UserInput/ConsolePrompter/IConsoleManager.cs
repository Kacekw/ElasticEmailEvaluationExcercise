using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserInput.ConsolePrompter
{
    public interface IConsoleManager
    {
        void WriteLine(string message, ConsoleColor consoleColor = ConsoleColor.White);
        void Write(string message, ConsoleColor consoleColor = ConsoleColor.White);
        string ReadLine();
    }
}

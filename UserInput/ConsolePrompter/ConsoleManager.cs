namespace UserInput.ConsolePrompter
{
    public class ConsoleManager : IConsoleManager
    {
        public string ReadLine()
        {
            return Console.ReadLine() ?? string.Empty;
        }

        public void Write(string message, ConsoleColor consoleColor = ConsoleColor.White)
        {
            var oldColor = Console.ForegroundColor;

            Console.ForegroundColor = consoleColor;
            Console.Write(message);

            Console.ForegroundColor = oldColor;
        }

        public void WriteLine(string message, ConsoleColor consoleColor = ConsoleColor.White)
        {
            var oldColor = Console.ForegroundColor;

            Console.ForegroundColor = consoleColor;
            Console.WriteLine(message);

            Console.ForegroundColor = oldColor;
        }
    }
}

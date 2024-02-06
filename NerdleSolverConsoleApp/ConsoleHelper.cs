namespace NerdleResearch
{
    public static class ConsoleHelper
    {
        public static void Write(string message, ConsoleColor color = ConsoleColor.DarkCyan)
        {
            Console.ForegroundColor = color;
            Console.Write(message);
        }

        public static void WriteLine(string message, ConsoleColor color = ConsoleColor.DarkCyan)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
        }
    }
}

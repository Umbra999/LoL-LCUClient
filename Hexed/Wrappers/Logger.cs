namespace Hexed.Wrappers
{
    internal class Logger
    {
        public static void Log(object obj)
        {
            if (obj == null) obj = "NULL";

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write($"[{DateTime.Now.ToShortTimeString()}] [");
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write("Hexed");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write($"] {obj}\n");
        }

        public static void LogError(object obj)
        {
            if (obj == null) obj = "NULL";

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write($"[{DateTime.Now.ToShortTimeString()}] [");
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write("Hexed");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write($"] ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"{obj}\n");
        }

        public static void LogWarning(object obj)
        {
            if (obj == null) obj = "NULL";

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write($"[{DateTime.Now.ToShortTimeString()}] [");
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write("Hexed");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write($"] ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"{obj}\n");
        }

        public static void LogDebug(object obj)
        {
            if (obj == null) obj = "NULL";

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write($"[{DateTime.Now.ToShortTimeString()}] [");
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write("Hexed");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write($"] ");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write($"{obj}\n");
        }
    }
}

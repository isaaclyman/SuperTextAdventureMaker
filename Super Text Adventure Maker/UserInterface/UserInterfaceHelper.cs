using System;

namespace Super_Text_Adventure_Maker.UserInterface
{
    public static class UserInterfaceHelper
    {
        public static string GetInput()
        {
            Console.Write("stam> ");
            return Console.ReadLine();
        }

        public static void OutputError(Exception error)
        {
            Console.Error.WriteLine(error.Message);
        }

        public static void OutputLine(string line)
        {
            Console.WriteLine(line);
        }

        public static void Pause()
        {
            Console.WriteLine();
            Console.WriteLine("[Press ENTER to continue.]");

            while (Console.ReadKey(true).Key != ConsoleKey.Enter)
            {
                // Block the thread until the Enter key is pressed.
            }
        }
    }
}

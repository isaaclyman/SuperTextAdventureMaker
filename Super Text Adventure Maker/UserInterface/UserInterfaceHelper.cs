using System;
using Super_Text_Adventure_Maker.Configuration;

namespace Super_Text_Adventure_Maker.UserInterface
{
    public static class UserInterfaceHelper
    {
        public static void ClearWindow()
        {
            Console.Clear();
        }

        public static string GetInput()
        {
            Console.Write(Strings.UserInterface_StamPrompt + @" ");
            var input = Console.ReadLine();
            Console.WriteLine();
            return input;
        }

        public static char GetNextKey()
        {
            Console.Write(Strings.UserInterface_StamPrompt + @" ");
            var key = Console.ReadKey(false).KeyChar;
            Console.WriteLine();
            Console.WriteLine();
            return key;
        }

        public static void OutputError(Exception error)
        {
            Console.Error.WriteLine(error.Message);
        }

        public static void OutputLine()
        {
            Console.WriteLine();
        }

        public static void OutputLine(string line)
        {
            Console.WriteLine(line);
        }

        public static void Pause()
        {
            Console.WriteLine();
            Console.WriteLine(Strings.UserInterface_PressEnterToContinue);

            while (Console.ReadKey(true).Key != ConsoleKey.Enter)
            {
                // Block the thread until the Enter key is pressed.
            }
        }
    }
}

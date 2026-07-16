using System;
using System.Collections.Generic;
using System.Text;

namespace Tinos3.Shell.Core
{
    public class CommandLineShell
    {
        public void MainCommandLine()
        {
            Console.Write("> ");
            var input = Console.ReadLine();

            if (string.IsNullOrEmpty(input))
                return;

            switch (input.ToLower())
            {
                case "help":
                    Console.WriteLine("Available commands:");
                    Console.WriteLine("  help     - Show this help message");
                    Console.WriteLine("  clear    - Clear the screen");

                    break;

                case "clear":
                    Console.Clear();
                    break;


                default:
                    Console.WriteLine($"\"{input}\" is not a command");
                    break;
            }
        }
    }
}

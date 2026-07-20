using System;
using System.Collections.Generic;
using System.Text;

using Tinos3.Abstractions.Commands;

namespace Tinos3.Shell.Commands
{
    public class HelpCmd : Command
    {
        public HelpCmd(string name) : base(name)
        {
        }

        public override string Execute(string[] args)
        {
            Console.Write("Page (0, 1, q): ");
            var input = Console.ReadLine();

            while (true)
            {
                switch (input)
                {
                    case "0":
                        showHelp(0);
                        break;

                    case "1":
                        showHelp(1);
                        break;

                    case "q":
                    case "quit":
                    case "exit":
                        break;

                    default:
                        Console.WriteLine("Unknown page.");
                        break;
                }
                return "";
            }
            
            
        }

        private void showHelp(int page) 
        {
            if (page == 0)
            {
                Console.WriteLine("General Commands. Page: " + page);

                Console.WriteLine("help     - Show this help message");
                Console.WriteLine("ver      - Show the version of the OS");
                Console.WriteLine("poweroff - Shutdown the system, fallsback to halt");
                Console.WriteLine("reboot   - Restart the system, fallsback to halt");
                Console.WriteLine("clear    - Clear the screen");
            } else if (page == 1)
            {
                Console.WriteLine("Filesystem related Commands. Page: " + page);


                Console.WriteLine("dir    - Show the files and directories of /mnt");
                Console.WriteLine("ls     - Same as the dir command");
                Console.WriteLine("format - Format the partion");
                Console.WriteLine("touch  - Create a file to a path");
                Console.WriteLine("nano   - Editor for editing files. a custom clone of GNU Nano");
                Console.WriteLine("mkdir  - Create directories/folders");
                Console.WriteLine("cat    - Read files from the disk");
                Console.WriteLine("copy   - copy files");
                Console.WriteLine("rm     - Delete folders");
                Console.WriteLine("del    - Delete files");
                Console.WriteLine("mv     - move files");
            }
            else
            {
                Console.WriteLine("Invalid Page: " + page);
                return;
            }

        }
    }
}

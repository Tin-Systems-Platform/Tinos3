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
            Console.WriteLine("help   - Show this help message");
            Console.WriteLine("ver    - Show the version of the OS");
            Console.WriteLine("dir    - Show the files and directories of /mnt");
            Console.WriteLine("ls     - Same as the dir command");
            Console.WriteLine("format - Format the partion");
            return "";
        }
    }
}

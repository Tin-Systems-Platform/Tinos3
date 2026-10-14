using System;
using System.Collections.Generic;
using System.Text;
using Tinos3.Shell.Core;

namespace Tinos3.Shell.Commands
{
    public class HelpCmd : Command
    {
        public HelpCmd(string name) : base(name)
        {
        }

        public override string Execute(string[] args)
        {
            Console.WriteLine("help - Show this help message");
            Console.WriteLine("ver  - Show the version of the OS");
            return "";
        }
    }
}

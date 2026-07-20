using System;
using System.Collections.Generic;
using System.Text;
using Tinos3.Abstractions.Commands;

namespace Tinos3.Shell.Commands
{
    public class ClearCmd : Command
    {
        public ClearCmd(string name) : base(name)
        {
        }

        public override string Execute(string[] args)
        {
            Console.Clear();
            return "";
        }
    }
}

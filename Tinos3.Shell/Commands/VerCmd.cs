using System;
using System.Collections.Generic;
using System.Text;
using Tinos3.Abstractions.Commands;

namespace Tinos3.Shell.Commands
{
    public class VerCmd : Command
    {
        public VerCmd(string name) : base(name)
        {
        }

        public override string Execute(string[] args)
        {
            Console.WriteLine("Tinos 3 Version: 0.1.0 Alpha 2");
            Console.WriteLine("Powered by Cosmos OS gen 3.");
            return "";
        }
    }
}

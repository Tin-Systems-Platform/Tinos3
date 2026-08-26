using System;
using System.Collections.Generic;
using System.Text;
using Tinos3.Abstractions.Commands;
using Tinos3.Abstractions.Common;

namespace Tinos3.Shell.Commands
{
    public class VerCmd : Command
    {
        public VerCmd(string name) : base(name)
        {
        }

        public override string Execute(string[] args)
        {
            Console.WriteLine("Tinos 3 Version: " + OsVersion.GetVersion("0.2.0 DEV 1"));
            Console.WriteLine("Powered by Cosmos OS gen 3.");
            return "";
        }
    }
}

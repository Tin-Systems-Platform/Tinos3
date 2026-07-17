using System;
using System.Collections.Generic;
using System.Text;
using Tinos3.Shell.Core;

namespace Tinos3.Shell.Commands
{
    public class TestCmd : Command
    {
        public TestCmd(string name) : base(name)
        {
        }

        public override string Execute(string[] args)
        {
            Console.WriteLine("Legacy code based from tinos2. This is a Test!!\n");
            return "";
        }
    }
}

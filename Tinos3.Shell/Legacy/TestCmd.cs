using System;
using System.Collections.Generic;
using System.Text;

namespace Tinos3.Shell.Legacy
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

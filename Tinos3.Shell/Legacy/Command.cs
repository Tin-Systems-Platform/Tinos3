using System;
using System.Collections.Generic;
using System.Text;

namespace Tinos3.Shell.Legacy
{
    public class Command
    {
        public readonly string name;

        public Command(string name)
        {
            this.name = name;
        }
        public virtual string Execute(string[] args) { return ""; }
    }
}

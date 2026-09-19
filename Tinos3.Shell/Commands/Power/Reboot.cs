using System;
using System.Collections.Generic;
using System.Text;
using Cosmos.Kernel.Core.X64.Power;
using Cosmos.Kernel.System;
using Tinos3.Abstractions.Commands;

namespace Tinos3.Shell.Commands.Power
{
    public class Reboot : Command
    {
       

        public Reboot(string name) : base(name)
        {
        }

        public override string Execute(string[] args)
        {
            Cosmos.Kernel.System.Power.Reboot();
            return "";
        }
    }
}

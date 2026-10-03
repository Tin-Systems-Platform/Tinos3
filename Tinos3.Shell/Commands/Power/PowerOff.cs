using Cosmos.Kernel.System;
using System;
using System.Collections.Generic;
using System.Text;
using Tinos3.Abstractions.Commands;

namespace Tinos3.Shell.Commands.Power
{
    public class PowerOff : Command
    {
        public PowerOff(string name) : base(name)
        {
        }

        public override string Execute(string[] args)
        {
            Cosmos.Kernel.System.Power.Shutdown();
            return base.Execute(args);
        }
    }
}

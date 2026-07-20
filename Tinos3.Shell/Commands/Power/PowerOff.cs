using Cosmos.Kernel.Core.X64.Power;
using System;
using System.Collections.Generic;
using System.Text;
using Tinos3.Abstractions.Commands;

namespace Tinos3.Shell.Commands.Power
{
    public class PowerOff : Command
    {
        X64PowerOps powerOps = new();

        public PowerOff(string name) : base(name)
        {
        }

        public override string Execute(string[] args)
        {
            powerOps.Shutdown();
            return base.Execute(args);
        }
    }
}

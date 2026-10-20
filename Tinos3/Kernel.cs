using System;
using Sys = Cosmos.Kernel.System;

namespace Tinos3
{
    /// <summary>
    /// Main kernel class - inherits from Cosmos.Kernel.System.Kernel.
    /// </summary>
    public class Kernel : Sys.Kernel
    {
        SystemInitializer sysInitializer = new SystemInitializer();

        protected override void BeforeRun()
        {
            sysInitializer.SysInit();
        }

        protected override void Run()
        {
            sysInitializer.SysRun();
        }
    }
}

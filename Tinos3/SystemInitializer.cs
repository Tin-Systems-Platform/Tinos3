using Cosmos.Executable.Lua;
using System;
using System.Collections.Generic;
using System.Text;
using Tinos3.Abstractions.Lua.Tinos3;

namespace Tinos3
{
    public class SystemInitializer
    {
        public LuaInterpreter lua;

        public void SysInit()
        {
            Console.WriteLine("[KERNEL]: Initializing system...");

            lua = new LuaInterpreter();

            Tinos3Lua.RegisterLuaFuncsToLuaInterpeter(lua);

            Console.WriteLine("[KERNEL]: System initialized.");
        }

        public void SysRun()
        {
            // BLANK: temporarely empty, will be used to be the Run method of the kernel, which will be called in the Kernel.Run() method.
        }
    }
}

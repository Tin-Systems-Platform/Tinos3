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
        public Shell.Shell shell = new Shell.Shell();

        public void SysInit()
        {
            Console.WriteLine("[KERNEL]: Initializing system...");

            lua = new LuaInterpreter();

            Tinos3Lua.RegisterLuaFuncsToLuaInterpeter(lua);

            Console.WriteLine("[KERNEL]: System initialized.");
        }

        public void SysRun()
        {
            shell.ShellMain();
        }
    }
}

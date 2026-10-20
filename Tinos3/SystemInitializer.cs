using Cosmos.Executable.Lua;
using System;
using System.Collections.Generic;
using System.Text;
using Tinos3.Abstractions.Lua.Tinos3;
using Tinos3.Abstractions.Utilities;
using Tinos3.Abstractions.Utilities.Panic;
using Tinos3.FS;

namespace Tinos3
{
    public class SystemInitializer
    {
        public LuaInterpreter lua;
        public Shell.Shell shell = new Shell.Shell();

        public void SysInit()
        {
            Console.WriteLine("[KERNEL]: Initializing system...");
            RegisterFSActions.InitFS();

            lua = new LuaInterpreter();

            Tinos3Lua.RegisterLuaFuncsToLuaInterpeter(lua);

            Console.WriteLine("[LUA]: Testing registered function...");
            lua.DoString("assert(type(GetTinosVersion) == 'function')");
            Console.WriteLine("[LUA]: Function exists!");
            Console.WriteLine("[LUA]: Testing function execution...");
            lua.DoString("assert(load('print(\"Hello, World!\")') ~= nil)");
            Console.WriteLine("[LUA]: Function executed successfully!"); 

            Console.WriteLine("[KERNEL]: System panic handler initialization started...");
            PanicHandler.Initialize();
            Console.WriteLine("[KERNEL]: System panic handler initialized successfully!");

            Tinos3.Abstractions.Utilities.Version.SetTinosVersion("Tinos3 0.2.0 the Rewritten Tinos3 beta 1");
            Console.WriteLine("[KERNEL]: System initialized.");
        }

        public void SysRun()
        {
            shell.ShellMain(lua);
        }
    }
}

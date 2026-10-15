using Cosmos.Executable.Lua;
using Cosmos.Kernel.System;
using System;
using System.Collections.Generic;
using System.Text;


namespace Tinos3.Abstractions.Lua.Tinos3.Utils
{
    public class PowerUtils
    {
        public static int Shutdown(ILuaState lua)
        {
            Power.Shutdown();
            return 0;
        }

        public static int Reboot(ILuaState lua)
        {
            Power.Reboot();
            return 0;
        }
    }
}

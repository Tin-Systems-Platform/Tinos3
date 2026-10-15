using Cosmos.Executable.Lua;
using System;
using System.Collections.Generic;
using System.Text;
using Tinos3.FS;

namespace Tinos3.Abstractions.Lua.Tinos3.FS
{
    public class DiskManagerLuaAPI
    {
        private static DiskMgr _diskMgr;
        public static int ListDisks(Cosmos.Executable.Lua.ILuaState lua)
        {
            _diskMgr = new DiskMgr();
            _diskMgr.ListDisks();
            return 1;
        }

        public static int GetDiskCount(ILuaState lua)
        {
            lua.PushString("Disk Count: " + _diskMgr.GetDiskCount());
            return 1;
        }
    }
}

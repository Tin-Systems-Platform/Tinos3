using Cosmos.Executable.Lua;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tinos3.Abstractions.Lua.Tinos3.Utils
{
    public class TinosUtils
    {
        private static string _tinosVersion = "Tinos3 0.2.0 The Rewritten Tinos3";

        public static int GetCurrentDateTime(ILuaState lua)
        {
            lua.PushString(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            return 1;
        }

        public static int SetTinosVersion(ILuaState lua)
        {
            string version = lua.ToString(-1);
            _tinosVersion = version;
            return 1;
        }

        public static int GetTinosVersion(ILuaState lua)
        {
            lua.PushString(_tinosVersion);
            return 1;
        }
    }
}

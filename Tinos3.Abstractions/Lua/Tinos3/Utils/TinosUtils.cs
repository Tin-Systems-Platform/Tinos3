using Cosmos.Executable.Lua;
using System;
using System.Collections.Generic;
using System.Text;
using TinosVersion = Tinos3.Api.Utilities.TVersion;

namespace Tinos3.Abstractions.Lua.Tinos3.Utils
{
    public class TinosUtils
    {
        public static int GetCurrentDateTime(ILuaState lua)
        {
            lua.PushString(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            return 1;
        }

        public static int SetTinosVersion(ILuaState lua)
        {
            string version = lua.ToString(-1);
            TinosVersion.SetTinosVersion(version);
            return 1;
        }

        public static int GetTinosVersion(ILuaState lua)
        {
            lua.PushString(TinosVersion.GetTinosVersion());
            return 1;
        }

        public static int ReadLine(ILuaState lua)
        {
            string input = Console.ReadLine();
            lua.PushString(input);
            return 1;
        }

        public static int Clear(ILuaState lua)
        {
            Console.Clear();
            return 0;
        }
    }
}

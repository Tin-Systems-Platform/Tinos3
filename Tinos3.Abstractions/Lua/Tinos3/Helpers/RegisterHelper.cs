using Cosmos.Executable.Lua;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tinos3.Abstractions.Lua.Tinos3.Helpers
{
    public class RegisterHelper
    {
        public static void RegisterCSharpFunctionToLua(ILuaState lua, CSharpFunctionDelegate function, string name)
        {
            lua.PushCSharpFunction(function);
            lua.SetGlobal(name);
        }
    }
}

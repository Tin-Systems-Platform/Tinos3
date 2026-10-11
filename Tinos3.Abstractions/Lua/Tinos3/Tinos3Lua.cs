using System;
using System.Collections.Generic;
using System.Text;
using Cosmos.Executable.Lua;

namespace Tinos3.Abstractions.Lua.Tinos3
{
    public class Tinos3Lua
    {
        private static void RegisterCSharpFunctionToLua(ILuaState lua, CSharpFunctionDelegate function, string name)
        {
            lua.PushCSharpFunction(function);
            lua.SetGlobal(name);
        }

        public static void RegisterLuaFuncsToLuaInterpeter(LuaInterpreter lua)
        {
            var luaState = lua.State;

            Console.WriteLine("[LUA]: Registering System API functions to Lua interpreter...");



            Console.WriteLine("[LUA]: System API functions registered.");
        }
    }
}

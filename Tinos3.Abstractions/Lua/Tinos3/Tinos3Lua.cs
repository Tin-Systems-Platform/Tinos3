using System;
using System.Collections.Generic;
using System.Text;
using Cosmos.Executable.Lua;
using Tinos3.Abstractions.Lua.Tinos3.Helpers;
using Tinos3.Abstractions.Lua.Tinos3.Registers;
using Tinos3.Abstractions.Lua.Tinos3.Utils;

namespace Tinos3.Abstractions.Lua.Tinos3
{
    public class Tinos3Lua
    {
        

        public static void RegisterLuaFuncsToLuaInterpeter(LuaInterpreter lua)
        {
            var luaState = lua.State;

            Console.WriteLine("[LUA]: Registering System API functions to Lua interpreter...");

            RegisterHelper.RegisterCSharpFunctionToLua(luaState, TinosUtils.GetCurrentDateTime, "GetCurrentDateTime");
            RegisterHelper.RegisterCSharpFunctionToLua(luaState, TinosUtils.SetTinosVersion, "SetTinosVersion");
            RegisterHelper.RegisterCSharpFunctionToLua(luaState, TinosUtils.GetTinosVersion, "GetTinosVersion");
            RegisterHelper.RegisterCSharpFunctionToLua(luaState, PowerUtils.Shutdown, "Shutdown");
            RegisterHelper.RegisterCSharpFunctionToLua(luaState, PowerUtils.Reboot, "Reboot");
            RegisterHelper.RegisterCSharpFunctionToLua(luaState, TinosUtils.ReadLine, "ReadLine");
            RegisterHelper.RegisterCSharpFunctionToLua(luaState, TinosUtils.Clear, "Clear");

            RegisterFSApi.RegisterFSApiToLuaInterpeter(luaState);

            Console.WriteLine("[LUA]: System API functions registered.");
        }
    }
}

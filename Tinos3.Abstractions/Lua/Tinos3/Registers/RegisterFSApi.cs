using System;
using System.Collections.Generic;
using System.Text;
using Tinos3.Abstractions.Lua.Tinos3.Helpers;
using Tinos3.Abstractions.Lua.Tinos3.FS;

namespace Tinos3.Abstractions.Lua.Tinos3.Registers
{
    public class RegisterFSApi
    {
        public static void RegisterFSApiToLuaInterpeter(Cosmos.Executable.Lua.ILuaState lua)
        {
            Console.WriteLine("[LUA]: Registering FS API functions to Lua interpreter...");
            RegisterHelper.RegisterCSharpFunctionToLua(lua, PartManagerLuaAPI.ListPartitions, "ListPartitions");
            RegisterHelper.RegisterCSharpFunctionToLua(lua, PartManagerLuaAPI.InitializePartitionTable, "InitializePartitionTable");
            Console.WriteLine("[LUA]: FS API functions registered.");
        }
    }
}

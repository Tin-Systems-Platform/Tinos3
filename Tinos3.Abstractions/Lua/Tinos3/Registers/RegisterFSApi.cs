using System;
using System.Collections.Generic;
using System.Text;
using Tinos3.Abstractions.Lua.Tinos3.Helpers;
using Tinos3.Abstractions.Lua.Tinos3.FS;
using Tinos3.FS.Helpers.Command;
using Cosmos.Executable.Lua;

namespace Tinos3.Abstractions.Lua.Tinos3.Registers
{
    public class RegisterFSApi
    {
        public static void RegisterFSApiToLuaInterpeter(Cosmos.Executable.Lua.ILuaState lua)
        {
            Console.WriteLine("[LUA]: Registering FS API functions to Lua interpreter...");

            InitKernelFSAPI(lua);
            InitUserFSAPI(lua);

            Console.WriteLine("[LUA]: FS API functions registered.");
        }

        private static void InitKernelFSAPI(ILuaState lua)
        {
            Console.WriteLine("[LUA]: Initializing Kernel FS API...");

            RegisterHelper.RegisterCSharpFunctionToLua(lua, PartManagerLuaAPI.ListPartitions, "ListPartitions");
            RegisterHelper.RegisterCSharpFunctionToLua(lua, PartManagerLuaAPI.InitializePartitionTable, "InitializePartitionTable");
            RegisterHelper.RegisterCSharpFunctionToLua(lua, PartManagerLuaAPI.CreatePartition, "CreatePartition");
            RegisterHelper.RegisterCSharpFunctionToLua(lua, PartManagerLuaAPI.DeletePartition, "DeletePartition");
            RegisterHelper.RegisterCSharpFunctionToLua(lua, PartManagerLuaAPI.ResizePartition, "ResizePartition");

            RegisterHelper.RegisterCSharpFunctionToLua(lua, DiskManagerLuaAPI.ListDisks, "ListDisks");
            RegisterHelper.RegisterCSharpFunctionToLua(lua, DiskManagerLuaAPI.GetDiskCount, "GetDiskCount");

            Console.WriteLine("[LUA]: Kernel FS API initialized.");
        }

        private static void InitUserFSAPI(ILuaState lua)
        {
            Console.WriteLine("[LUA]: Initializing User facing FS API...");
            RegisterHelper.RegisterCSharpFunctionToLua(lua, FDisk.LuaFDiskMain, "FDiskMain");
            Console.WriteLine("[LUA]: User facing FS API initialized.");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Tinos3.Abstractions.Lua.Tinos3.Helpers;
using Tinos3.Abstractions.Lua.Tinos3.FS;
using Tinos3.FS.Helpers.Command;
using Cosmos.Executable.Lua;
using Tinos3.Abstractions.Applications.Nano;

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

            RegisterHelper.RegisterCSharpFunctionToLua(lua, FSCmds.ListFiles, "ListFiles");
            RegisterHelper.RegisterCSharpFunctionToLua(lua, FSCmds.ReadFile, "ReadFile");
            RegisterHelper.RegisterCSharpFunctionToLua(lua, FSCmds.CopyFile, "CopyFile");


            RegisterHelper.RegisterCSharpFunctionToLua(lua, FSCmds.CreateDirectory, "MakeDirectory");
            RegisterHelper.RegisterCSharpFunctionToLua(lua, FSCmds.DeleteDirectory, "RemoveDirectory");
            RegisterHelper.RegisterCSharpFunctionToLua(lua, FSCmds.DeleteFile, "RemoveFile");
            RegisterHelper.RegisterCSharpFunctionToLua(lua, FSCmds.MoveFile, "MoveFile");
            RegisterHelper.RegisterCSharpFunctionToLua(lua, FSCmds.MoveDirectory, "MoveDirectory");
            RegisterHelper.RegisterCSharpFunctionToLua(lua, FSCmds.RenameFile, "RenameFile");
            RegisterHelper.RegisterCSharpFunctionToLua(lua, FSCmds.RenameDirectory, "RenameDirectory");
            RegisterHelper.RegisterCSharpFunctionToLua(lua, FSCmds.MakeFile, "MakeFile");
            RegisterHelper.RegisterCSharpFunctionToLua(lua, NanoMain.LuaRun, "NanoMain");
            Console.WriteLine("[LUA]: User facing FS API initialized.");
        }
    }
}

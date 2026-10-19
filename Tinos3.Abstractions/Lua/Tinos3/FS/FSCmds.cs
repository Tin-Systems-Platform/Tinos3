using Cosmos.Executable.Lua;
using System;
using System.Collections.Generic;
using System.Text;
using CommandMethods = Tinos3.FS.Helpers.Command;

namespace Tinos3.Abstractions.Lua.Tinos3.FS
{
    public class FSCmds
    {
        public static int ListFiles(ILuaState lua)
        {
            string path = lua.ToString(-1);
            bool hasColor = lua.ToBoolean(-2);
            CommandMethods.File.ListFiles(path, hasColor);
            return 0;
        }

        public static int ReadFile(ILuaState lua)
        {
            string path = lua.ToString(-1);
            CommandMethods.File.ReadFile(path);
            return 0;
        }

        public static int CopyFile(ILuaState lua)
        {
            string sourcePath = lua.ToString(-1);
            string destinationPath = lua.ToString(-2);
            bool overwrite = lua.ToBoolean(-3);
            CommandMethods.File.CopyFile(sourcePath, destinationPath, overwrite);
            return 0;
        }


    }
}

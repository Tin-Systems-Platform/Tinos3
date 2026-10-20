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
            string path = lua.ToString(1);
            bool hasColor = lua.ToBoolean(2);

            CommandMethods.File.ListFiles(path, hasColor);
            return 0;
        }

        public static int ReadFile(ILuaState lua)
        {
            string path = lua.ToString(1);
            CommandMethods.File.ReadFile(path);
            return 0;
        }

        public static int CopyFile(ILuaState lua)
        {
            string sourcePath = lua.ToString(1);
            string destinationPath = lua.ToString(2);
            bool overwrite = lua.ToBoolean(3);
            CommandMethods.File.CopyFile(sourcePath, destinationPath, overwrite);
            return 0;
        }

        public static int DeleteFile(ILuaState lua)
        {
            string path = lua.ToString(1);
            CommandMethods.File.RemoveFile(path);
            return 0;
        }

        public static int MoveFile(ILuaState lua)
        {
            string sourcePath = lua.ToString(1);
            string destinationPath = lua.ToString(2);
            CommandMethods.File.MoveFile(sourcePath, destinationPath);
            return 0;
        }

        public static int CreateDirectory(ILuaState lua)
        {
            string path = lua.ToString(1);
            CommandMethods.File.MakeDirectory(path);
            return 0;
        }

        public static int DeleteDirectory(ILuaState lua)
        {
            string path = lua.ToString(1);
            CommandMethods.File.RemoveDirectory(path);
            return 0;
        }

        public static int MoveDirectory(ILuaState lua)
        {
            string sourcePath = lua.ToString(1);
            string destinationPath = lua.ToString(2);
            CommandMethods.File.MoveDirectory(sourcePath, destinationPath);
            return 0;
        }

        public static int RenameFile(ILuaState lua)
        {
            string oldPath = lua.ToString(1);
            string newPath = lua.ToString(2);
            CommandMethods.File.RenameFile(oldPath, newPath);
            return 0;
        }

        public static int RenameDirectory(ILuaState lua)
        {
            string oldPath = lua.ToString(1);
            string newPath = lua.ToString(2);
            CommandMethods.File.RenameDirectory(oldPath, newPath);
            return 0;
        }

        public static int MakeFile(ILuaState lua)
        {
            string path = lua.ToString(1);
            CommandMethods.File.MakeFile(path);
            return 0;
        }



        public static int LoadScript(ILuaState lua)
        {
            string path = lua.ToString(1);

            if (string.IsNullOrWhiteSpace(path))
            {
                lua.PushNil();
                lua.PushString("No file specified.");
                return 2;
            }

            string fullPath = CommandMethods.File.ResolvePath(path);

            if (!System.IO.File.Exists(fullPath))
            {
                lua.PushNil();
                lua.PushString($"File '{path}' doesn't exist.");
                return 2;
            }

            try
            {
                string contents = System.IO.File.ReadAllText(fullPath);
                lua.PushString(contents);
                return 1;
            }
            catch (Exception ex)
            {
                lua.PushNil();
                lua.PushString($"Unable to read file '{path}': {ex.Message}");
                return 2;
            }
        }
    }
}

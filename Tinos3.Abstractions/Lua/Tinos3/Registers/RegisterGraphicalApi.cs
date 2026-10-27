using Cosmos.Executable.Lua;
using Tinos3.Abstractions.Lua.Tinos3.Helpers;

namespace Tinos3.Abstractions.Lua.Tinos3.Registers
{
    class RegisterGraphicalApi
    {
        public static void RegisterGraphicalApiToLuaInterpeter(ILuaState lua)
        {
            Console.WriteLine("[LUA]: Registering Graphical API functions to Lua interpreter...");

            //RegisterHelper.RegisterCSharpFunctionToLua(lua, graphical.Graphical, "DesktopInitialize");

            Console.WriteLine("[LUA]: Graphical API functions registered.");
        }
    }
}
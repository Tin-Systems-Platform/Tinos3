using Cosmos.Executable.Lua;
using Tinos3.Abstractions.Lua.Tinos3.Apps;
using Tinos3.Abstractions.Lua.Tinos3.Helpers;
using Tinos3.Abstractions.Lua.Tinos3.Utils;

namespace Tinos3.Abstractions.Lua.Tinos3.Registers;

public static class RegisterApplicationApi
{
    public static void RegisterApplicationApiToLuaInterpreter(ILuaState lua)
    {
        Console.WriteLine("[LUA]: Registering application API functions to Lua interpreter...");

        // Alias the existing date/time implementation for application scripts.
        RegisterHelper.RegisterCSharpFunctionToLua(lua, TinosUtils.GetCurrentDateTime, "GetCurrentTime");
        RegisterHelper.RegisterCSharpFunctionToLua(lua, ApplicationLuaApi.GetCurrentTimeString, "GetCurrentTimeString");
        RegisterHelper.RegisterCSharpFunctionToLua(lua, ApplicationLuaApi.GetCurrentDateString, "GetCurrentDateString");

        Console.WriteLine("[LUA]: Application API functions registered.");
    }
}

using Cosmos.Executable.Lua;
using Tinos3.Api.Utilities;

namespace Tinos3.Abstractions.Lua.Tinos3.Apps;

/// <summary>Lua wrappers for utility functions intended for application scripts.</summary>
public static class ApplicationLuaApi
{
    public static int GetCurrentTimeString(ILuaState lua)
    {
        lua.PushString(Clock.GetCurrentTimeString());
        return 1;
    }

    public static int GetCurrentDateString(ILuaState lua)
    {
        lua.PushString(Clock.GetCurrentDateString());
        return 1;
    }
}

using Tinos3.Graphical.Desktop;
using Cosmos.Executable.Lua;

namespace Tinos3.Abstractions.Lua.Tinos3.graphical
{
    class Graphical
    {
        public static int DesktopInitialize(ILuaState lua)
        {
            DesktopInitializer.Initialize();
            return 0;
        }
    }
}
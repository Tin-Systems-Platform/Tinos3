using Tinos3.Graphical.Desktop;
using Cosmos.Executable.Lua;

namespace Tinos3.Abstractions.Lua.Tinos3.graphical
{
    class Graphical
    {
        public static int DesktopInitialize(ILuaState lua)
        {
            // TODO: do not instantly exit out of the desktop
            // FIXME: This only triggers the desktop initialization, but does not keep it running. The desktop should remain active until the user exits it.
            DesktopInitializer.Initialize();
            return 0;
        }
    }
}
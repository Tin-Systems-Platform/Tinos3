
using Cosmos.Executable.Lua;
using Tinos3.Graphical.Desktop;

namespace Tinos3.Abstractions.Lua.Tinos3.graphical
{
    class Graphical
    {
        public static int DesktopInitialize(ILuaState lua)
        {
            // TODO: do not instantly exit out of the desktop
            // FIXME: Add new desktop initialization code here
            Desktop desktop = new Desktop();
            desktop.initDesktop();
            desktop.ActiveRenderLoop();
            return 0;
        }
    }
}
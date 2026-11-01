
using Cosmos.Executable.Lua;
using Tinos3.Graphical.Desktop;

namespace Tinos3.Abstractions.Lua.Tinos3.graphical
{
    class Graphical
    {
        public static int DesktopInitialize(ILuaState lua)
        {
            string startupCallback = null;
            if (!lua.IsNoneOrNil(1))
            {
                if (!lua.IsString(1))
                {
                    lua.PushNil();
                    lua.PushString("Usage: startx([startupCallbackName])");
                    return 2;
                }

                startupCallback = lua.ToString(1);
            }

            Desktop desktop = new Desktop();
            desktop.initDesktop(showExampleWindows: string.IsNullOrEmpty(startupCallback));
            GraphicalLuaApi.Initialize(lua, desktop.WindowManager);
            GraphicalLuaApi.InvokeStartupCallback(startupCallback);
            desktop.ActiveRenderLoop();
            return 0;
        }
    }
}

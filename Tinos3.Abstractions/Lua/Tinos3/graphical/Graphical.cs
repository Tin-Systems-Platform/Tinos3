
using Cosmos.Executable.Lua;
using Tinos3.Api.Internal;
using Tinos3.Graphical.Desktop;
using Tinos3.Api.Graphical.Windows;

namespace Tinos3.Abstractions.Lua.Tinos3.graphical
{
    class Graphical
    {
        private static GlobalVars globalVars = new GlobalVars();

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

            if(!globalVars.isDesktopRunning)
            {
                Desktop desktop = new Desktop();
                desktop.initDesktop(showExampleWindows: string.IsNullOrEmpty(startupCallback));
                
                GraphicalLuaApi.Initialize(lua, desktop.WindowManager);
                GraphicalLuaApi.InvokeStartupCallback(startupCallback);
                
                desktop.ActiveRenderLoop();
                return 0;
            }

            WindowManager windowManager = new WindowManager();


            GraphicalLuaApi.InvokeStartupCallback(startupCallback);

            return 0;
        }
    }
}

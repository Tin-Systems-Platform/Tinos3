

using Cosmos.Kernel.System;
using Tinos3.Scripts;

namespace Tinos3.Shell
{
    public class Shell
    {
        public void ShellMain(Cosmos.Executable.Lua.LuaInterpreter lua)
        {
            var lshScript = ScriptResources.Get("Tinos3.Scripts.shell.shellMain.lua");
            var lshScriptString = System.Text.Encoding.UTF8.GetString(lshScript);

            lua.DoString(lshScriptString);

        }
    }
}



using Cosmos.Kernel.System;
using Tinos3.Scripts;

namespace Tinos3.Shell
{
    public class Shell
    {
        public void ShellMain(Cosmos.Executable.Lua.LuaInterpreter lua)
        {
            Console.WriteLine("[SHELL]: Shell started.");
            Console.WriteLine("Type 'help' for a list of commands.");
            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine();
                if (input == "poweroff")
                {
                    Console.WriteLine("SHUTTING DOWN THE SYSTEM...");
                    Power.Shutdown();
                    break;
                }
                else if (input == "help")
                {
                    Console.WriteLine("Available commands:");
                    Console.WriteLine("help - Show this help message.");
                    Console.WriteLine("poweroff - Shut down the system.");
                } else if (input == "ver")
                {
                    var verScript = ScriptResources.Get("Tinos3.Scripts.ver.lua");
                    string verScriptString = System.Text.Encoding.UTF8.GetString(verScript);
                    lua.DoString(verScriptString);
                }
                else
                {
                    Console.WriteLine($"Unknown command: {input}");
                }
            }

        }
    }
}

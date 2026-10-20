using Cosmos.Executable.Lua;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tinos3.Abstractions.Applications.Nano
{
    /// <summary>
    /// The main class for the nano text editor.
    /// </summary>
    public class NanoMain
    {
        public void Run()
        {
            NanoBuffer buffer = new NanoBuffer();
            int cursorX = 0;
            int cursorY = 0;
            ConsoleKeyInfo keyInfo;
            do
            {
                Console.Clear();
                for (int i = 0; i < buffer.LineCount; i++)
                {
                    Console.WriteLine(buffer.GetLine(i));
                }
                Console.SetCursorPosition(cursorX, cursorY);
                keyInfo = Console.ReadKey(intercept: true);
                NanoInput.HandleShortcut(keyInfo, buffer, ref cursorX, ref cursorY);
            } while (keyInfo.Key != ConsoleKey.Escape); // Exit on Escape key
        }

        public static int LuaRun(ILuaState lua)
        {
            NanoMain nano = new NanoMain();
            nano.Run();
            return 0;
        }
    }
}

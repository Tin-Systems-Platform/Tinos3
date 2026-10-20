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
        public void Run(string path)
        {
            NanoBuffer buffer = new NanoBuffer();

            if (System.IO.File.Exists(path))
            {
                buffer.SetText(NanoFileIO.ReadFile(path));
            }

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

                // Time to have a save and exit option
                if (keyInfo.Key == ConsoleKey.S && keyInfo.Modifiers.HasFlag(ConsoleModifiers.Control))
                {
                    // Save the buffer to the specified path
                    NanoFileIO.WriteFile(path, buffer.GetText(), overwrite: true);
                    Console.WriteLine("\nFile saved. Press any key to continue...");
                    Console.ReadKey(intercept: true);
                }
                if (keyInfo.Key == ConsoleKey.Q && keyInfo.Modifiers.HasFlag(ConsoleModifiers.Control))
                {
                    // Exit the editor
                    break;
                }
                if (keyInfo.Key == ConsoleKey.C && keyInfo.Modifiers.HasFlag(ConsoleModifiers.Control))
                {
                    // line amount
                    Console.WriteLine($"\nLine count: {buffer.LineCount}.");
                }

                NanoInput.HandleShortcut(keyInfo, buffer, ref cursorX, ref cursorY);

               
                // Ensure cursor stays within bounds
                if (cursorY < 0) cursorY = 0;
                if (cursorY >= buffer.LineCount) cursorY = buffer.LineCount - 1;
                if (cursorX < 0) cursorX = 0;
                if (cursorY < buffer.LineCount && cursorX > buffer.GetLine(cursorY).Length)
                {
                    cursorX = buffer.GetLine(cursorY).Length;
                }
            } while (keyInfo.Key != ConsoleKey.Escape); // Exit on Escape key
        }

        public static int LuaRun(ILuaState lua)
        {
            string path = lua.ToString(1); // Get the first argument from Lua
            NanoMain nano = new NanoMain();
            nano.Run(path);
            return 0;
        }
    }
}

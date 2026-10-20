
using Cosmos.Executable.Lua;
using System;

namespace Tinos3.Abstractions.Applications.Nano
{
    public class NanoMain
    {
        private const int ConsoleWidth = 80;
        private const int ConsoleHeight = 25;

        public void Run(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                Console.WriteLine("No file specified.");
                return;
            }

            NanoBuffer buffer = new NanoBuffer();
            buffer.SetText(NanoFileIO.ReadFile(path));

            string savedText = buffer.GetText();

            int cursorX = 0;
            int cursorY = 0;


            while (true)
            {
                Console.Clear();

                NanoRenderer.Draw(
                    path,
                    buffer,
                    savedText,
                    cursorX,
                    cursorY);

                ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);

                if (keyInfo.Key == ConsoleKey.S &&
                    keyInfo.Modifiers.HasFlag(ConsoleModifiers.Control))
                {
                    NanoFileIO.WriteFile(
                        path,
                        buffer.GetText(),
                        overwrite: true);

                    // Update after a confirmed successful save.
                    // NanoFileIO should return success/failure for this.
                    savedText = buffer.GetText();
                    continue;
                }

                if (keyInfo.Key == ConsoleKey.Q &&
                    keyInfo.Modifiers.HasFlag(ConsoleModifiers.Control))
                {
                    break;
                }

                if (keyInfo.Key == ConsoleKey.C &&
                    keyInfo.Modifiers.HasFlag(ConsoleModifiers.Control))
                {
                    Console.SetCursorPosition(0, ConsoleHeight - 2);
                    Console.Write($"Lines: {buffer.LineCount}. Press a key...");
                    Console.ReadKey(intercept: true);
                    continue;
                }

                if (keyInfo.Key == ConsoleKey.Escape)
                    break;

                NanoInput.HandleShortcut(
                    keyInfo,
                    buffer,
                    ref cursorX,
                    ref cursorY);

                cursorY = Math.Clamp(cursorY, 0, buffer.LineCount - 1);
                cursorX = Math.Clamp(
                    cursorX, 0, buffer.GetLine(cursorY).Length);
            }

        }

        public static int LuaRun(ILuaState lua)
        {
            string path = lua.ToString(1);

            if (string.IsNullOrWhiteSpace(path))
            {
                path = "default.txt";
                return 0;
            }

            NanoMain nano = new NanoMain();
            nano.Run(path);
            return 0;
        }
    }
}

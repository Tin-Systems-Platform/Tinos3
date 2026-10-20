
using System;

namespace Tinos3.Abstractions.Applications.Nano
{
    public static class NanoRenderer
    {
        private const int ConsoleWidth = 80;
        private const int ConsoleHeight = 25;

        private const int HeaderRow = 0;
        private const int EditorStartRow = 1;
        private const int StatusRow = ConsoleHeight - 2;
        private const int ShortcutsRow = ConsoleHeight - 1;

        private const int LineNumberWidth = 5;

        public static void Draw(
            string path,
            NanoBuffer buffer,
            string savedText,
            int cursorX,
            int cursorY)
        {
            bool modified = buffer.GetText() != savedText;

            // NanoMain should clear the console once before calling Draw.
            DrawHeader(path, modified);
            DrawBuffer(buffer);
            DrawStatus(cursorX, cursorY);
            DrawShortcuts();

            // Restore the cursor to its document position.
            int screenX = Math.Clamp(
                LineNumberWidth + cursorX,
                0,
                ConsoleWidth - 1);

            int screenY = Math.Clamp(
                EditorStartRow + cursorY,
                EditorStartRow,
                StatusRow - 1);

            Console.SetCursorPosition(screenX, screenY);
        }

        private static void DrawHeader(string path, bool modified)
        {
            Console.SetCursorPosition(0, HeaderRow);

            string title = $" Tinos3 Nano - {path}";
            if (modified)
                title += " [MODIFIED]";

            WriteClipped(title, ConsoleWidth);
        }

        private static void DrawBuffer(NanoBuffer buffer)
        {
            int editorHeight = StatusRow - EditorStartRow;
            int availableTextWidth = ConsoleWidth - LineNumberWidth;

            for (int row = 0; row < editorHeight; row++)
            {
                Console.SetCursorPosition(0, EditorStartRow + row);

                if (row < buffer.LineCount)
                {
                    string line = buffer.GetLine(row);
                    string lineNumber = $"{row + 1,4} ";

                    Console.Write(lineNumber);
                    WriteClipped(line, availableTextWidth);
                }
            }
        }

        private static void DrawStatus(int cursorX, int cursorY)
        {
            Console.SetCursorPosition(0, StatusRow);

            string status = $" Ln {cursorY + 1}, Col {cursorX + 1}";
            WriteClipped(status, ConsoleWidth);
        }

        private static void DrawShortcuts()
        {
            Console.SetCursorPosition(0, ShortcutsRow);

            WriteClipped(
                " ^S Save   ^Q Quit   ^C Line count",
                ConsoleWidth);
        }

        private static void WriteClipped(string text, int maxWidth)
        {
            if (maxWidth <= 0)
                return;

            Console.Write(
                text.Length > maxWidth
                    ? text.Substring(0, maxWidth)
                    : text);
        }
    }
}

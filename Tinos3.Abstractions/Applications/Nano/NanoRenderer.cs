
using System;

namespace Tinos3.Abstractions.Applications.Nano
{
    public static class NanoRenderer
    {
        public static void DrawHeader(string path, bool modified)
        {
            Console.SetCursorPosition(0, 0);
            ClearLine();

            string title = $" Tinos3 Nano - {path}";
            if (modified)
                title += " [MODIFIED]";

            WriteClipped(title, Console.WindowWidth);
        }

        public static void DrawBuffer(
            NanoBuffer buffer,
            int cursorY,
            int cursorX)
        {
            int width = Console.WindowWidth;
            int height = Console.WindowHeight;
            int editorHeight = height - 3;

            for (int row = 0; row < editorHeight; row++)
            {
                Console.SetCursorPosition(0, row + 1);
                ClearLine();

                int lineIndex = row;

                if (lineIndex >= buffer.LineCount)
                    continue;

                string line = buffer.GetLine(lineIndex);
                string lineNumber = $"{lineIndex + 1,4} ";

                Console.Write(lineNumber);
                WriteClipped(
                    line,
                    Math.Max(0, width - lineNumber.Length));
            }

            // Position the cursor in the document area.
            if (editorHeight > 0)
            {
                int visibleY = Math.Clamp(cursorY, 0, editorHeight - 1);
                int visibleX = Math.Min(
                    width - 1,
                    5 + Math.Max(0, cursorX));

                Console.SetCursorPosition(
                    Math.Max(0, visibleX),
                    visibleY + 1);
            }
        }

        public static void DrawStatus(
            NanoBuffer buffer,
            string savedText,
            int cursorX,
            int cursorY)
        {
            int width = Console.WindowWidth;
            int row = Console.WindowHeight - 2;

            Console.SetCursorPosition(0, row);
            ClearLine();

            bool modified = buffer.GetText() != savedText;

            string status =
                $" Ln {cursorY + 1}, Col {cursorX + 1}";

            if (modified)
                status += " [MODIFIED]";

            WriteClipped(status, width);
        }

        public static void DrawShortcuts()
        {
            int width = Console.WindowWidth;

            Console.SetCursorPosition(
                0,
                Console.WindowHeight - 1);

            ClearLine();

            WriteClipped(
                " ^S Save   ^Q Quit   ^C Line count",
                width);
        }

        private static void ClearLine()
        {
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(
                0,
                Console.CursorTop);
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

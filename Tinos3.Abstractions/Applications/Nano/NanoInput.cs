using System;
using System.Collections.Generic;
using System.Text;

namespace Tinos3.Abstractions.Applications.Nano
{
    
    public class NanoInput
    {
        /// <summary>
        /// This method handles all of the shortcuts for the nano text editor. It takes a key input and performs the corresponding action.
        /// </summary>
        public static void HandleShortcut(ConsoleKeyInfo keyInfo, NanoBuffer buffer, ref int cursorX, ref int cursorY)
        {
            switch (keyInfo.Key)
            {
                case ConsoleKey.Backspace:
                    if (cursorX > 0)
                    {
                        buffer.DeleteChar(cursorX - 1, cursorY);
                        cursorX--;
                    }
                    else if (cursorY > 0)
                    {
                        cursorY--;
                        cursorX = buffer.GetLine(cursorY).Length;
                        buffer.JoinLines(cursorY);
                    }
                    break;
                case ConsoleKey.Enter:
                    buffer.InsertNewLine(cursorX, cursorY);
                    cursorY++;
                    cursorX = 0;
                    break;
                case ConsoleKey.LeftArrow:
                    if (cursorX > 0)
                        cursorX--;
                    else if (cursorY > 0)
                    {
                        cursorY--;
                        cursorX = buffer.GetLine(cursorY).Length;
                    }
                    break;
                case ConsoleKey.RightArrow:
                    if (cursorX < buffer.GetLine(cursorY).Length)
                        cursorX++;
                    else if (cursorY < buffer.LineCount - 1)
                    {
                        cursorY++;
                        cursorX = 0;
                    }
                    break;
                case ConsoleKey.UpArrow:
                    if (cursorY > 0)
                        cursorY--;
                    break;
                case ConsoleKey.DownArrow:
                    if (cursorY < buffer.LineCount - 1)
                        cursorY++;
                    break;
                default:
                    // Handle character input
                    if (!char.IsControl(keyInfo.KeyChar))
                    {
                        buffer.InsertChar(cursorX, cursorY, keyInfo.KeyChar);
                        cursorX++;
                    }
                    break;
            }
        }
    }
}

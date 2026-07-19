using System;
using System.Collections.Generic;
using System.Text;
using Tinos3.Applications.Categories;
using Tinos3.Applications.Utils;

namespace Tinos3.Applications.Utilities.Editor
{
    public class Editor : UtilitiesCategory
    {

        static List<string> buffer = new List<string> { "" };
        static int cursorX = 0, cursorY = 0;
        static bool running = true;
        string filename = "default.txt"; // Default filename

        public Editor(string categoryName, string description) : base(categoryName, description)
        {
        }

        public  void Edit(string filename)
        {
            this.filename = filename;

            Console.Clear();
            Console.WriteLine("Nano Clone - Basic Text Editor for Cosmos");
            Console.WriteLine("Ctrl+X: Exit | Ctrl+O: Save | Ctrl+L: Load | Ctrl+S: Set Filename");
            Console.WriteLine("------------------------------------------------");
            LoadFile(); // Try loading the default file

            while (running)
            {
                Render();
                HandleInput();
            }
        }

         void Render()
        {
            Console.SetCursorPosition(0, 3);
            Console.Write(new string(' ', Console.WindowWidth * (Console.WindowHeight - 3))); // Clear screen area

            for (int i = 0; i < buffer.Count; i++)
            {
                Console.SetCursorPosition(0, i + 3);
                Console.Write(buffer[i]);
            }

            Console.SetCursorPosition(0, Console.WindowHeight - 2);
            Console.Write($"[Editing: {filename}]".PadRight(Console.WindowWidth)); // Show current filename

            Console.SetCursorPosition(cursorX, cursorY + 3);
        }

         void HandleInput()
        {
            var key = Console.ReadKey(true);

            if (key.Modifiers == ConsoleModifiers.Control)
            {
                switch (key.Key)
                {
                    case ConsoleKey.X:
                        running = false;
                        break;
                    case ConsoleKey.O:
                        SaveFile();
                        break;
                    case ConsoleKey.L:
                        LoadFile();
                        break;
                    case ConsoleKey.S:
                        SetFilename();
                        break;
                }
                return;
            }

            switch (key.Key)
            {
                case ConsoleKey.Backspace:
                    if (cursorX > 0)
                    {
                        buffer[cursorY] = buffer[cursorY].Remove(cursorX - 1, 1);
                        cursorX--;
                    }
                    break;

                case ConsoleKey.Enter:
                    buffer.Insert(cursorY + 1, "");
                    cursorY++;
                    cursorX = 0;
                    break;

                case ConsoleKey.LeftArrow:
                    if (cursorX > 0) cursorX--;
                    break;

                case ConsoleKey.RightArrow:
                    if (cursorX < buffer[cursorY].Length) cursorX++;
                    break;

                case ConsoleKey.UpArrow:
                    if (cursorY > 0) cursorY--;
                    cursorX = Math.Min(cursorX, buffer[cursorY].Length);
                    break;

                case ConsoleKey.DownArrow:
                    if (cursorY < buffer.Count - 1) cursorY++;
                    cursorX = Math.Min(cursorX, buffer[cursorY].Length);
                    break;

                default:
                    buffer[cursorY] = buffer[cursorY].Insert(cursorX, key.KeyChar.ToString());
                    cursorX++;
                    break;
            }
        }

         void SaveFile()
        {
            try
            {
                string path = @"/mnt" + filename;
                string fileContent = string.Join("\n", buffer);

                if (!File.Exists(path))
                {
                    File.Create(path);
                }

                System.IO.File.WriteAllText(path, fileContent);
                ShowMessage($"File saved as {filename}");
            }
            catch (Exception ex)
            {
                ShowMessage($"Save failed: {ex.Message}");
            }
        }

         void LoadFile()
        {
            try
            {
                string path = @"/mnt" + filename;

                if (System.IO.File.Exists(path))
                {
                    string fileContent = System.IO.File.ReadAllText(path);
                    buffer = new List<string>(fileContent.Split('\n'));
                    cursorX = 0;
                    cursorY = 0;
                    ShowMessage($"File {filename} loaded.");
                }
                else
                {
                    ShowMessage("No file found.");
                }
            }
            catch (Exception ex)
            {
                ShowMessage($"Load failed: {ex.Message}");
            }
        }

         void SetFilename()
        {
            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            Console.Write("Enter new filename: ".PadRight(Console.WindowWidth));
            Console.SetCursorPosition(18, Console.WindowHeight - 1);

            string input = ReadLine();
            if (!string.IsNullOrWhiteSpace(input))
            {
                filename = input.Trim();
                ShowMessage($"Filename set to {filename}");
            }
        }

         string ReadLine()
        {
            string input = "";
            ConsoleKeyInfo key;

            while ((key = Console.ReadKey(true)).Key != ConsoleKey.Enter)
            {
                if (key.Key == ConsoleKey.Backspace && input.Length > 0)
                {
                    input = input.Substring(0, input.Length - 1);
                    Console.Write("\b \b");
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    input += key.KeyChar;
                    Console.Write(key.KeyChar);
                }
            }
            return input;
        }

         void ShowMessage(string message)
        {
            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            Console.Write(message.PadRight(Console.WindowWidth));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Tinos3.Abstractions.Applications.Nano
{
    public class NanoBuffer
    {
        private readonly List<string> lines = new() { string.Empty };
        public int LineCount => lines.Count;

        public void InsertChar(int x, int y, char c)
        {
            if (y < 0 || x < 0)
                throw new ArgumentOutOfRangeException();

            while (y >= lines.Count)
                lines.Add(string.Empty);

            if (x > lines[y].Length)
                throw new ArgumentOutOfRangeException(nameof(x));

            lines[y] = lines[y].Insert(x, c.ToString());
        }

        public void DeleteChar(int x, int y)
        {
            if (y < 0 || y >= lines.Count || x < 0 || x >= lines[y].Length)
                throw new ArgumentOutOfRangeException();

            lines[y] = lines[y].Remove(x, 1);
        }

        public void InsertNewLine(int x, int y)
        {
            if (y < 0 || y >= lines.Count ||
                x < 0 || x > lines[y].Length)
            {
                throw new ArgumentOutOfRangeException();
            }

            string currentLine = lines[y];
            string newLine = currentLine.Substring(x);

            lines[y] = currentLine.Substring(0, x);
            lines.Insert(y + 1, newLine);
        }

        public void JoinLines(int y)
        {
            if (y < 0 || y >= lines.Count - 1)
                throw new ArgumentOutOfRangeException();

            lines[y] += lines[y + 1];
            lines.RemoveAt(y + 1);
        }

        public string GetLine(int y)
        {
            if (y < 0 || y >= lines.Count)
                throw new ArgumentOutOfRangeException();

            return lines[y];
        }

        public void SetLine(int y, string content)
        {
            if (y < 0 || y >= lines.Count)
                throw new ArgumentOutOfRangeException();

            lines[y] = content;
        }

    }
}

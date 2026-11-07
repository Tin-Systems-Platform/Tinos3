using Cosmos.Kernel.System.Graphics;
using Cosmos.Kernel.System.Graphics.Fonts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Tinos3.Api.Graphical.Widgets
{
    public class Label
    {
        public int X { get; set; }

        public int Y { get; set; }

        public string Text { get; set; }

        public void Draw(Canvas canvas)
        {
            Draw(canvas, Color.Black);
        }

        public void Draw(Canvas canvas, Color color)
        {
            if (Text == null)
            {
                return;
            }

            int lineStart = 0;
            int lineY = Y;

            for (int i = 0; i <= Text.Length; i++)
            {
                if (i < Text.Length && Text[i] != '\n')
                {
                    continue;
                }

                int lineLength = i - lineStart;
                // Treat Windows CRLF and Unix LF line endings identically.
                if (lineLength > 0 && Text[lineStart + lineLength - 1] == '\r')
                {
                    lineLength--;
                }

                if (lineLength > 0)
                {
                    string line = Text.Substring(lineStart, lineLength);
                    canvas.DrawString(line, PCScreenFont.DefaultFont, color, X, lineY);
                }

                lineStart = i + 1;
                lineY += 25;
            }
        }
    }
}

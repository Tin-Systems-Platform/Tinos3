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
            // Screen font glyphs are approximately 8 pixels wide and 16 high.
            canvas.DrawString(Text, PCScreenFont.DefaultFont, Color.Black, X, Y);
        }

        public void Draw(Canvas canvas, Color color)
        {
            // Screen font glyphs are approximately 8 pixels wide and 16 high.
            canvas.DrawString(Text, PCScreenFont.DefaultFont, color, X, Y);
        }
    }
}

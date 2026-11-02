using Cosmos.Kernel.System.Graphics;
using Cosmos.Kernel.System.Graphics.Fonts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Tinos3.Api.Graphical.Widgets
{
    //TODO: Implement text input handling for the TextBox widget.
    public class TextBox
    {
        public int X { get; set; }

        public int Y { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public string Text { get; set; }

        public void Draw(Canvas canvas)
        {
            // Draw the text box background and border
            canvas.DrawFilledRectangle(Color.White, X, Y, Width, Height);
            canvas.DrawRectangle(Color.Black, X, Y, Width, Height);
            // Draw the text inside the text box
            // Screen font glyphs are approximately 8 pixels wide and 16 high.
            int textX = X + 5; // Add some padding from the left edge
            int textY = Y + ((Height - 16) / 2); // Center the text vertically
            canvas.DrawString(Text, PCScreenFont.DefaultFont, Color.Black, textX, textY);
        }
    }
}

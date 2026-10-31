using System;
using Cosmos.Kernel.System;
using Cosmos.Kernel.System.Graphics;
using Cosmos.Kernel.System.Graphics.Fonts;
using System.Drawing;

namespace Tinos3.Api.Graphical.Widgets;

public class ButtonWidget
{
    public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Text { get; set; }
        public Action OnClick { get; set; }

        public bool IsHovered(int mouseX, int mouseY)
        {
            return mouseX >= X && mouseX <= X + Width && mouseY >= Y && mouseY <= Y + Height;
        }

        public void Draw(Canvas canvas, bool isHovered, bool isPressed)
        {
            Color bgColor = isPressed ? Color.DarkGray : (isHovered ? Color.LightGray : Color.White);
            canvas.DrawFilledRectangle(bgColor, X, Y, Width, Height);
            canvas.DrawRectangle(Color.Black, X, Y, Width, Height);

            // Simple centering approximation for text
            int textX = X + (Width / 2) - (Text.Length * 4);
            int textY = Y + (Height / 2) - 8;
            canvas.DrawString(Text, PCScreenFont.DefaultFont, Color.Black, textX, textY);
        }

}
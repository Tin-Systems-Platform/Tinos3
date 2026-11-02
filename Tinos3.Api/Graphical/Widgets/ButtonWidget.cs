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
        public bool IsHoveredState { get; private set; }
        public bool IsPressed { get; private set; }
        private bool _wasPressed;

        public bool IsHovered(int mouseX, int mouseY)
        {
            return mouseX >= X && mouseX <= X + Width && mouseY >= Y && mouseY <= Y + Height;
        }

        /// <summary>Updates hover and click state from coordinates local to the window canvas.</summary>
        public void HandleInput(int mouseX, int mouseY, bool leftButtonDown)
        {
            IsHoveredState = IsHovered(mouseX, mouseY);
            IsPressed = leftButtonDown && IsHoveredState;

            if (_wasPressed && !leftButtonDown && IsHoveredState)
            {
                OnClick?.Invoke();
            }

            _wasPressed = leftButtonDown && IsHoveredState;
        }

        public void Draw(Canvas canvas)
        {
            Color bgColor = IsPressed ? Color.DarkGray : (IsHoveredState ? Color.LightGray : Color.White);
            canvas.DrawFilledRectangle(bgColor, X, Y, Width, Height);
            canvas.DrawRectangle(Color.Black, X, Y, Width, Height);

            // Screen font glyphs are approximately 8 pixels wide and 16 high.
            int textX = X + (Width / 2) - (Text.Length * 8);
            int textY = Y + ((Height - 16) / 2);
            canvas.DrawString(Text, PCScreenFont.DefaultFont, Color.Black, textX, textY);
        }

    public void Draw(Canvas canvas, Color bgColor1)
    {
        Color bgColor = IsPressed ? bgColor1 : (IsHoveredState ? Color.LightGray : Color.White);
        canvas.DrawFilledRectangle(bgColor, X, Y, Width, Height);
        canvas.DrawRectangle(Color.Black, X, Y, Width, Height);
        // Screen font glyphs are approximately 8 pixels wide and 16 high.
        int textX = X + (Width / 2) - (Text.Length * 8);
        int textY = Y + ((Height - 16) / 2);
        canvas.DrawString(Text, PCScreenFont.DefaultFont, Color.Black, textX, textY);
    }

}

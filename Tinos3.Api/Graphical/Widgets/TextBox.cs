using Cosmos.Kernel.System.Graphics;
using System;
using System.Drawing;

namespace Tinos3.Api.Graphical.Widgets
{
    public class TextBox
    {
        private const int HorizontalPadding = 5;
        private const int GlyphWidth = 10;
        private bool _wasPressed;

        public int X { get; set; }

        public int Y { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public bool IsFocused { get; private set; }

        public string Text { get; private set; } = string.Empty;

        public bool IsHovered(int mouseX, int mouseY)
        {
            return mouseX >= X && mouseX <= X + Width && mouseY >= Y && mouseY <= Y + Height;
        }

        public void HandleInput(int mouseX, int mouseY, bool leftButtonDown)
        {
            bool isHovered = IsHovered(mouseX, mouseY);

            if (leftButtonDown && !_wasPressed)
            {
                IsFocused = isHovered;
            }

            _wasPressed = leftButtonDown;
        }

        public void HandleInput(ConsoleKeyInfo keyInfo)
        {
            if (!IsFocused)
            {
                return;
            }

            if (keyInfo.Key == ConsoleKey.Backspace)
            {
                if (Text.Length > 0)
                {
                    Text = Text[..^1];
                }

                return;
            }

            if (!char.IsControl(keyInfo.KeyChar) && Text.Length < GetVisibleCharacterCapacity())
            {
                Text += keyInfo.KeyChar;
            }
        }

        public void SetText(string text)
        {
            Text = text ?? string.Empty;
            if (Text.Length > GetVisibleCharacterCapacity())
            {
                Text = Text[..GetVisibleCharacterCapacity()];
            }
        }

        public void Draw(Canvas canvas)
        {
            canvas.DrawFilledRectangle(Color.White, X, Y, Width, Height);
            canvas.DrawRectangle(IsFocused ? Color.Blue : Color.Black, X, Y, Width, Height);

            int textX = X + HorizontalPadding;
            int textY = Y + ((Height - GraphicalFont.DefaultSize) / 2);
            GraphicalFont.DrawString(canvas, Text, Color.Black, textX, textY);
        }

        private int GetVisibleCharacterCapacity()
        {
            int availableWidth = Math.Max(0, Width - (HorizontalPadding * 2));
            return Math.Max(0, availableWidth / GlyphWidth);
        }
    }
}

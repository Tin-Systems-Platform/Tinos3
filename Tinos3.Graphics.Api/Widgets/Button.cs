using Cosmos.Kernel.System.Graphics;
using Cosmos.Kernel.System.Graphics.Fonts;
using Cosmos.Kernel.System.Mouse;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using cosmosGraphics = Cosmos.Kernel.System.Graphics;

namespace Tinos3.Graphics.Api.Widgets
{
    public class Button
    {
        private Canvas _canvas;

        private Font _font;

        private void createButton(string text, int x, int y, int width, int height)
        {
            _canvas = Canvas.GetFullScreen();
            _font = PCScreenFont.DefaultFont;

            _canvas.DrawFilledRectangle(Color.LightGray, x, y, width, height);

            _canvas.DrawString(text, _font, Color.Black, x + width / 6, y - height / 10);
        }

        public Button(string text, int x, int y, int width, Action action)
        {
            createButton(text, x, y, width, 32);
            ButtonClicked(action, x, y);
        }

        public Button(string text, int x, int y, int width, int height, Action action)
        {
            createButton(text, x, y, width, height);
            ButtonClicked(action, x, y);
        }

        public void ButtonClicked(Action action, int x, int y)
        {
            // Logic for button click 
            bool isMouseXInside = (MouseManager.X >= x) && (MouseManager.X <= x + 100);
            bool isMouseYInside = (MouseManager.Y >= y) && (MouseManager.Y <= y + 32);

            if (isMouseXInside && isMouseYInside)
            {
                if (MouseManager.LeftButton)
                {
                    action.Invoke();
                }
            }
        }
    }
}

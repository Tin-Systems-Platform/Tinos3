using Cosmos.Kernel.System.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using cosmosGraphics = Cosmos.Kernel.System.Graphics;
using Cosmos.Kernel.System.Graphics.Fonts;
using System.Drawing;

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

        public Button(string text, int x, int y, int width)
        {
            createButton(text, x, y, width, 32);
        }

        public Button(string text, int x, int y, int width, int height)
        {
            createButton(text, x, y, width, height);
        }
    }
}

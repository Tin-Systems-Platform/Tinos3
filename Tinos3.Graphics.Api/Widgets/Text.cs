using System;
using System.Collections.Generic;
using System.Text;
using Cosmos.Kernel.System.Graphics;
using Cosmos.Kernel.System.Graphics.Fonts;
using System.Drawing;

namespace Tinos3.Graphics.Api.Widgets
{
    public class Text
    {
        Canvas _canvas = Canvas.GetFullScreen();
        Font _font = PCScreenFont.DefaultFont;
        public Text(string text, int x, int y)
        {
            _canvas.DrawString(text, _font, Color.Black, x, y);

        }
    }
}

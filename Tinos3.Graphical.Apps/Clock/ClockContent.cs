using System;
using Tinos3.Api.Graphical.Windows;
using TClock = Tinos3.Api.Utilities;
using Cosmos.Kernel.System.Graphics;
using Cosmos.Kernel.System.Graphics.Fonts;
using System.Drawing;

namespace Tinos3.Graphical.Apps.Clock
{
    public class ClockContent : IWindowContent
    {
        public string WindowTitle(string title)
        {
            return "Clock";
        }

        public void DrawContent(Canvas canvas)
        {
            canvas.DrawString(
                "Time: " + TClock.Clock.GetCurrentTimeString(),
                PCScreenFont.DefaultFont,
                Color.Black,
                10,
                40
            );
        }
    }
}

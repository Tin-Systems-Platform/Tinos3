using System;
using Tinos3.Api.Graphical.Windows;
using TClock = Tinos3.Api.Utilities;
using Cosmos.Kernel.System.Graphics;
using System.Drawing;
using Tinos3.Api.Graphical.Widgets;

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
            GraphicalFont.DrawString(canvas, "Time: " + TClock.Clock.GetCurrentTimeString(), Color.Black, 10, 40);
        }
    }
}

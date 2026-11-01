using Cosmos.Kernel.System.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Tinos3.Api.Graphical.Windows;

namespace Tinos3.Graphical.Desktop.ExampleWindow
{
    internal class ExampleWindow : IWindowContent
    {
        public void DrawContent(Canvas canvas)
        {
            // Draw to the window's own canvas so the content is clipped to and
            // moves with its window when the window is dragged.
            canvas.DrawCircle(Color.Red, 100, 100, 50);
        }

        public string WindowTitle(string title)
        {
            return title;
        }
    }
}

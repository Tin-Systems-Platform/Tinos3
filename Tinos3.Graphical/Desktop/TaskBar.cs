using Cosmos.Kernel.System.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Tinos3.Graphical.Desktop
{
    internal class TaskBar
    {
        Canvas _canvas;


        public void TaskBarRenderer(Canvas canvas)
        {
            canvas = Canvas.GetFullScreen();
            _canvas = canvas;

            // Taskbar background

            _canvas.DrawFilledRectangle(Color.Gray, 0, _canvas.Height - 32, _canvas.Width, 32);
        }
    }
}

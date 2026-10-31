using Cosmos.Kernel.System.Graphics;
using Cosmos.Kernel.System.Graphics.Fonts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Tinos3.Graphical.Desktop
{
    internal class TaskBar
    {

        public void TaskBarRenderer(Canvas canvas)
        {
            canvas = Canvas.GetFullScreen();


            // Taskbar background

            canvas.DrawFilledRectangle(Color.Gray, 0, canvas.Height - 32, canvas.Width, 32);

            canvas.DrawFilledRectangle(Color.Green, 0, canvas.Height - 32, 80, 32);
            canvas.DrawString("Start", PCScreenFont.DefaultFont, Color.White, 12, canvas.Height - 28);
        }
    }
}

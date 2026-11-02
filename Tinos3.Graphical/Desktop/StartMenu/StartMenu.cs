using Cosmos.Kernel.System.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Tinos3.Api.Graphical.Widgets;

namespace Tinos3.Graphical.Desktop.StartMenu
{
    public class StartMenu
    {
        public void RenderStartMenu(Canvas canvas, int x, int y)
        {
            // Render the start menu background
            canvas.DrawFilledRectangle(Color.LightGray, x, y, 200, 300);
            // Render the start menu items
            Label startMenuLabel = new Label
            {
                X = x + 10,
                Y = y + 10,
                Text = "Start Menu"
            };
            startMenuLabel.Draw(canvas);
            // Additional menu items can be added here
        }
    }
}

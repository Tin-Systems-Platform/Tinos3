using Cosmos.Kernel.System.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Tinos3.Api.Graphical.Widgets;
using Tinos3.Api.Graphical.Windows;
using Tinos3.Graphical.Apps.Clock;

namespace Tinos3.Graphical.Desktop.StartMenu
{
    public class StartMenu : IWindowInputHandler
    {
        private readonly ButtonWidget _clockAppLaunch;
        private readonly int _x;
        private readonly int _y;

        public StartMenu(WindowManager windowManager, int x, int y)
        {
            _x = x;
            _y = y;
            _clockAppLaunch = new ButtonWidget
            {
                X = x + 20,
                Y = y + 35,
                Width = 200,
                Height = 40,
                Text = "Open Clock App",
                OnClick = () =>
                {
                    Window clockWindow = new Window(100, 100, 300, 200, "Clock", new ClockContent());
                    windowManager.AddWindow(clockWindow);
                }
            };
        }

        public void HandleInput(int mouseX, int mouseY, bool leftButtonDown)
        {
            _clockAppLaunch.HandleInput(mouseX, mouseY, leftButtonDown);
        }

        public void RenderStartMenu(Canvas canvas)
        {
            // Render the start menu background
            canvas.DrawFilledRectangle(Color.LightGray, _x, _y, 400, 300);
            // Render the start menu items
            Label startMenuLabel = new Label
            {
                X = _x + 10,
                Y = _y + 10,
                Text = "Start Menu"
            };
            startMenuLabel.Draw(canvas);
            _clockAppLaunch.Draw(canvas);
            // Additional menu items can be added here
        }
    }
}

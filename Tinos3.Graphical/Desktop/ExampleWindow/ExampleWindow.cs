using Cosmos.Kernel.System.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Tinos3.Api.Graphical.Widgets;
using Tinos3.Api.Graphical.Windows;
using Tinos3.Graphical.Apps.Clock;

namespace Tinos3.Graphical.Desktop.ExampleWindow
{
    internal class ExampleWindow : IWindowContent, IWindowInputHandler
    {
        private readonly WindowManager _windowManager;
        private readonly ButtonWidget _openClock;

        public ExampleWindow(WindowManager windowManager)
        {
            _windowManager = windowManager;
            _openClock = new ButtonWidget
            {
                X = 50,
                Y = 150,
                Width = 200,
                Height = 50,
                Text = "Open Clock",
                OnClick = () =>
                {
                    ClockContent clockContent = new ClockContent();
                    _windowManager.AddWindow(new Window(200, 200, 300, 200, "Clock", clockContent));
                }
            };
        }

        public void HandleInput(int mouseX, int mouseY, bool leftButtonDown)
        {
            _openClock.HandleInput(mouseX, mouseY, leftButtonDown);
        }

        public void DrawContent(Canvas canvas)
        {
            // Draw to the window's own canvas so the content is clipped to and
            // moves with its window when the window is dragged.
            canvas.DrawCircle(Color.Red, 100, 100, 50);
            _openClock.Draw(canvas);
        }

        public string WindowTitle(string title)
        {
            return title;
        }
    }
}

using Cosmos.Kernel.System.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Tinos3.Graphics.Desktop
{
    internal class StartMenu
    {
        private Png _startButton;
        private Png _taskBar;
        private Canvas _canvas;

        private Png _taskbarSeperator;

        private Png _mousePointer;



        private void PopulateStartMenu()
        {
            _startButton = new Png("/mnt/gui/StartButton.png");
            _taskBar = new Png("/mnt/gui/Taskbar.png");
            _taskbarSeperator = new Png("/mnt/gui/TaskbarSeparator.png");

            _mousePointer = new Png("/mnt/gui/MouseCursor.png");

            int startButtonPadding = 5;
            int taskbarPadding = 0;

            int startButtonX = 0 + startButtonPadding;
            int startButtonY = (int)_canvas.Height - (int)_startButton.Height - startButtonPadding;

            int taskbarX = 0 + taskbarPadding;
            int taskbarY = (int)_canvas.Height - (int)_taskBar.Height - taskbarPadding;

            string[] apps = new String[5];

            apps.SetValue("shutdown", 0);

            _canvas.DrawFilledRectangle(Color.White, startButtonX - startButtonPadding, startButtonY + startButtonPadding, 240, 40);
        }

        public void renderStartMenu() {
            PopulateStartMenu();

        }
    }
}

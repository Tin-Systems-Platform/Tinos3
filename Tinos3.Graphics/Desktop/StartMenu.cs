using Cosmos.Kernel.System.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Cosmos.Kernel.System.Graphics.Fonts;

namespace Tinos3.Graphics.Desktop
{
    internal class StartMenu
    {
        private Png _startButton;
        private Png _powerButton;
        private Canvas _canvas;

        private Font _font;


        private void PopulateStartMenu()
        {
            _canvas = Canvas.GetFullScreen();

            _font = PCScreenFont.DefaultFont;
            _startButton = new Png("/mnt/gui/StartButton.png");
            _powerButton = new Png("/mnt/gui/PowerButton.png");


            int startButtonPadding = 5;
            int taskbarPadding = 0;

            int startButtonX = 0 + startButtonPadding;
            int startButtonY = (int)_canvas.Height - (int)_startButton.Height - startButtonPadding - 50;

            int startMenuBgX = startButtonX - startButtonPadding;
            int startMenuBgY = startButtonY + startButtonPadding;

            string[] apps = new String[5];

            apps.SetValue("shutdown", 0);

            _canvas.DrawFilledRectangle(Color.White, startMenuBgX, startMenuBgY - 120, 300, 160);
            _canvas.DrawImage(_powerButton, startMenuBgX + 5,5 - startMenuBgY, _powerButton.Width / 2, _powerButton.Height / 2);
            _canvas.DrawString("Shutdown", _font, Color.Black, startButtonX - startButtonPadding + 50, startButtonY + startButtonPadding);


        }

        public void renderStartMenu() {
            PopulateStartMenu();

        }
    }
}

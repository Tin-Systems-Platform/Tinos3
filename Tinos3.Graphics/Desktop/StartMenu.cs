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
        private Canvas _canvas;

        private Font _font;


        private void PopulateStartMenu()
        {
            _canvas = Canvas.GetFullScreen();
            
            _font = PCScreenFont.DefaultFont;
            _startButton = new Png("/mnt/gui/StartButton.png");


            int startButtonPadding = 5;
            int taskbarPadding = 0;

            int startButtonX = 0 + startButtonPadding;
            int startButtonY = (int)_canvas.Height - (int)_startButton.Height - startButtonPadding;

            string[] apps = new String[5];

            apps.SetValue("shutdown", 0);

            _canvas.DrawFilledRectangle(Color.White, startButtonX - startButtonPadding, startButtonY + startButtonPadding, 240, 40);
            _canvas.DrawString("Shutdown", _font, Color.Black, startButtonX - startButtonPadding, startButtonY + startButtonPadding);
        }

        public void renderStartMenu() {
            PopulateStartMenu();

        }
    }
}

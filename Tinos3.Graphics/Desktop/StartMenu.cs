using Cosmos.Kernel.System;
using Cosmos.Kernel.System.Graphics;
using Cosmos.Kernel.System.Graphics.Fonts;
using Cosmos.Kernel.System.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

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

            int powerbuttonY = startMenuBgY - 5;
            int powerbuttonX = startMenuBgX + 5;

            string[] apps = new String[5];

            apps.SetValue("shutdown", 0);

            _canvas.DrawFilledRectangle(Color.White, startMenuBgX, startMenuBgY - 120, 300, 160);
            _canvas.DrawImage(_powerButton, startMenuBgX + 5, startMenuBgY - 5, _powerButton.Width / 2, _powerButton.Height / 2);
            _canvas.DrawString("Shutdown", _font, Color.Black, startButtonX - startButtonPadding + 50, startButtonY + startButtonPadding);

            bool isOnTopOfPowerButton = false;

            

            bool isMouseXInsidePowerButton = (MouseManager.X >= powerbuttonX) && (MouseManager.X <= powerbuttonX + 40);
            bool isMouseYInsidePowerButton = (MouseManager.Y >= powerbuttonY) && (MouseManager.Y <= powerbuttonY + 40);

            if (isMouseXInsidePowerButton && isMouseYInsidePowerButton)
            {
                isOnTopOfPowerButton = true;
            }

            if (isOnTopOfPowerButton)
            {
                if (MouseManager.LeftButton)
                {
                    Power.Shutdown();
                }
            }

        }

        public void renderStartMenu() {
            PopulateStartMenu();

        }
    }
}

using Cosmos.Kernel.System.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Cosmos.Kernel.System.Mouse;
using Cosmos.Kernel.System.Graphics.Fonts;
using Cosmos.Kernel.System.Keyboard;
using Cosmos.Kernel.System.Keyboard.ScanMaps;
using Tinos3.Graphics.Api.Widgets;

namespace Tinos3.Graphics.Desktop
{
    public class InitDesktop
    {
        private Png _startButton;
        private Png _taskBar;
        private Canvas _canvas;

        private Png _taskbarSeperator;

        private Png _mousePointer;
        private StartMenu _startMenu;
        private bool _isStartMenuOpen = false;

        private void LoadAssets()
        {
            _startMenu = new StartMenu();

            _startButton = new Png("/mnt/gui/StartButton.png");
            _taskBar = new Png("/mnt/gui/Taskbar.png");
            _taskbarSeperator = new Png("/mnt/gui/TaskbarSeparator.png");

            _mousePointer = new Png("/mnt/gui/MouseCursor.png");

            _canvas = Canvas.GetFullScreen();

            MouseManager.SetScreenSize(_canvas.Width, _canvas.Height);
        }

        internal void RenderDesktop(Boolean dontClear, Boolean dontLog)
        {

            if (dontClear && dontLog)
            {
                new Button("TEST", 10, 10, 100);
                int startButtonPadding = 5;
                int taskbarPadding = 0;

                int startButtonX = 0 + startButtonPadding;
                int startButtonY = (int)_canvas.Height - (int)_startButton.Height - startButtonPadding;

                int taskbarX = 0 + taskbarPadding;
                int taskbarY = (int)_canvas.Height - (int)_taskBar.Height - taskbarPadding;

                int taskbarSeparatorX = 44 + startButtonX;
                int taskbarSeparatorY = startButtonY - 5;

                int startMenuBgX = startButtonX - startButtonPadding;
                int startMenuBgY = startButtonY + startButtonPadding;

                _canvas.DrawImage(_taskBar, taskbarX, taskbarY);
                _canvas.DrawImage(_startButton, startButtonX, startButtonY);
                _canvas.DrawImage(_taskbarSeperator, taskbarSeparatorX, taskbarSeparatorY);

                

                bool isOnTopOfStartButton = false;


                bool isMouseXInside = (MouseManager.X >= startButtonX) && (MouseManager.X <= startButtonX + 32);
                bool isMouseYInside = (MouseManager.Y >= startButtonY) && (MouseManager.Y <= startButtonY + 32);

                bool isMouseXInsideStartMenu = (MouseManager.X >= startMenuBgX) && (MouseManager.X <= startMenuBgX + 40);
                bool isMouseYInsideStartMenu = (MouseManager.Y >= startMenuBgY) && (MouseManager.Y <= startMenuBgY + 40);

                if (isMouseXInside && isMouseYInside)
                {
                    isOnTopOfStartButton = true;
                }

                if(isOnTopOfStartButton)
                {
                    if(MouseManager.LeftButton)
                    {
                        _isStartMenuOpen = true;
                    }
                }

                if (!isOnTopOfStartButton)
                {
                    if (!isMouseXInsideStartMenu || !isMouseYInsideStartMenu)
                    {
                        if (MouseManager.LeftButton)
                        {
                            _isStartMenuOpen = false;
                        }
                    }
                }

                if (_isStartMenuOpen)
                {
                    _startMenu.renderStartMenu();
                }

                _canvas.DrawImage(_mousePointer, MouseManager.X, MouseManager.Y, _mousePointer.Height / 4, _mousePointer.Width / 4);

                _canvas.Display();
            }
            else
            {
                Console.WriteLine("GRAPHICS: Desktop initializing");
                _canvas.Clear(Color.DarkBlue);

                _canvas.DrawImage(_startButton, 0, 0);
                _canvas.Display();
            }
        }




        public void ShowDesktop(Boolean indefinitely)
        {

            LoadAssets();



            if (indefinitely)
            {

                while (true)
                {
                    _canvas.Clear(Color.DarkBlue);

                    RenderDesktop(true, true);

                    if (KeyboardManager.KeyAvailable)
                    {
                        KeyEvent key = KeyboardManager.ReadKey();
                        if (key.Key == ConsoleKeyEx.Escape)
                        {
                            Console.Clear();
                            Console.WriteLine("Exiting Desktop");
                            break;
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Not showing desktop indefinitely");
                RenderDesktop(true, true);
            }
        }
    }
}

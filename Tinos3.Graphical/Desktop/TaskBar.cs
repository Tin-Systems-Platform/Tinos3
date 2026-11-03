using Cosmos.Kernel.System.Graphics;
using Cosmos.Kernel.System.Graphics.Fonts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Tinos3.Api.Graphical.Widgets;
using Cosmos.Kernel.System.Input;

namespace Tinos3.Graphical.Desktop
{
    internal class TaskBar
    {
        private readonly ButtonWidget _startButton;
        private readonly StartMenu.StartMenu _startMenu;
        private readonly int _screenHeight;
        private bool _isStartMenuOpen;

        public TaskBar(int screenHeight, WindowManager windowManager)
        {
            _screenHeight = screenHeight;
            _startMenu = new StartMenu.StartMenu(windowManager, 0, screenHeight - 32 - 300, () => _isStartMenuOpen = false);

            _startButton = new ButtonWidget
            {
                X = 0,
                Y = screenHeight - 32,
                Width = 100,
                Height = 32,
                Text = "Start",
                OnClick = () => _isStartMenuOpen = !_isStartMenuOpen
            };
        }

        public void HandleInput()
        {
            _startButton.HandleInput((int)MouseManager.X, (int)MouseManager.Y, MouseManager.LeftButton);
            if (_isStartMenuOpen)
            {
                _startMenu.HandleInput((int)MouseManager.X, (int)MouseManager.Y, MouseManager.LeftButton);
            }
        }

        public void Render(Canvas canvas)
        {
            canvas.DrawFilledRectangle(Color.DarkCyan, 0, _screenHeight - 32, canvas.Width, 32);
            _startButton.Draw(canvas);

            if (_isStartMenuOpen)
            {
                _startMenu.RenderStartMenu(canvas);
            }
        }
    }
}

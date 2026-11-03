using Cosmos.Kernel.System.Graphics;
using Cosmos.Kernel.System.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Tinos3.Api.Graphical.Widgets;
using Tinos3.Api.Graphical.Windows;
using Tinos3.Graphical.Apps.Clock;
using Tinos3.Graphical.Apps.System.About;
using Tinos3.Graphical.Apps.System.PowerManagement;

namespace Tinos3.Graphical.Desktop.StartMenu
{
    public class StartMenu : IWindowInputHandler
    {
        private readonly ButtonWidget _clockAppLaunch;

        private readonly ButtonWidget _aboutAppLaunch;

        private readonly ButtonWidget _powerMgrAppLaunch;

        private readonly Action _closeStartMenu;

        private readonly int _x;
        private readonly int _y;

        public StartMenu(WindowManager windowManager, int x, int y, Action closeStartMenu)
        {
            _x = x;
            _y = y;
            _closeStartMenu = closeStartMenu;


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
                    _closeStartMenu();
                }
            };

            _aboutAppLaunch = new ButtonWidget
            {
                X = x + 20,
                Y = y + 85,
                Width = 200,
                Height = 40,
                Text = "About Tinos3",
                OnClick = () =>
                {
                    AboutAppContent aboutContent = new AboutAppContent(windowManager.RemoveWindow);
                    Window aboutWindow = new Window(150, 150, 700, 300, "About Tinos3", aboutContent);
                    aboutContent.SetWindow(aboutWindow);
                    windowManager.AddWindow(aboutWindow);
                    _closeStartMenu();

                }
            };

            _powerMgrAppLaunch = new ButtonWidget
            {
                X = x + 20,
                Y = y + 135,
                Width = 200,
                Height = 40,
                Text = "Power Management",
                OnClick = () =>
                {
                    Window powerWindow = new Window(150, 150, 400, 250, "Power Management", new PowerManagementContent());
                    windowManager.AddWindow(powerWindow);
                    _closeStartMenu();
                }
            };

        }

        public void HandleInput(int mouseX, int mouseY, bool leftButtonDown)
        {
            // Close the start menu when clicking outside of it.
            bool isMouseXInside = mouseX >= _x && mouseX <= _x + 400;
            bool isMouseYInside = mouseY >= _y && mouseY <= _y + 300;

            if (leftButtonDown && (!isMouseXInside || !isMouseYInside))
            {
                _closeStartMenu();
                return;
            }

            _clockAppLaunch.HandleInput(mouseX, mouseY, leftButtonDown);
            _aboutAppLaunch.HandleInput(mouseX, mouseY, leftButtonDown);
            _powerMgrAppLaunch.HandleInput(mouseX, mouseY, leftButtonDown);
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
            _aboutAppLaunch.Draw(canvas);
            _powerMgrAppLaunch.Draw(canvas);
            // Additional menu items can be added here
        }
    }
}

using Cosmos.Kernel.System.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Tinos3.Api.Graphical.Widgets;
using Tinos3.Api.Graphical.Windows;
using Cosmos.Kernel.System.Diagnostics;

namespace Tinos3.Graphical.Apps.System.About
{
    public class AboutAppContent : IWindowContent, IWindowInputHandler
    {
        private readonly Action<Window> _removeWindow;
        private Window _aboutWindow;
        private readonly ButtonWidget _okButton;

        public AboutAppContent(Action<Window> removeWindow)
        {
            _removeWindow = removeWindow;

            _okButton = new ButtonWidget
            {
                X = 20,
                Y = 170,
                Width = 100,
                Height = 30,
                Text = "OK",
                OnClick = () =>
                {
                    if (_aboutWindow != null)
                        _removeWindow(_aboutWindow);
                }
            };
        }

        public void SetWindow(Window window)
        {
            _aboutWindow = window;
        }

        public void DrawContent(Canvas canvas)
        {
            Label aboutLabel = new Label
            {
                X = 20,
                Y = 35,
                Text = "Tinos3 OS\nVersion: 0.2.0 beta 3\nDeveloped by: randomusert and contributors\n\n"
            };

            aboutLabel.Draw(canvas);

            Label licenseLabel = new Label
            {
                X = 20,
                Y = 130,
                Text = "License: MIT License"
            };

            licenseLabel.Draw(canvas);
            _okButton.Draw(canvas);
        }

        public void HandleInput(int mouseX, int mouseY, bool leftButtonDown)
        {
            _okButton.HandleInput(mouseX, mouseY, leftButtonDown);
        }

        public string WindowTitle(string title)
        {
            return "About Tinos3";
        }
    }
}

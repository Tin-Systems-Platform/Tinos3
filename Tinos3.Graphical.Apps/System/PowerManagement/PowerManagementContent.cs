using Cosmos.Kernel.System;
using Cosmos.Kernel.System.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Tinos3.Api.Graphical.Widgets;
using Tinos3.Api.Graphical.Windows;
using Tinos3.Api.Utilities;

namespace Tinos3.Graphical.Apps.System.PowerManagement
{
    public class PowerManagementContent : IWindowContent, IWindowInputHandler
    {
        ButtonWidget _shutdownButton;
        ButtonWidget _restartButton;
        ButtonWidget _haltButton;
        ButtonWidget _panicButton;

        public void DrawContent(Canvas canvas)
        {
            Label powerManagement = new Label
            {
                X = 10,
                Y = 10,
                Text = "Power Management"
            };

            powerManagement.Draw(canvas);

            Label optionLabel = new Label
            {
                X = 10,
                Y = 40,
                Text = "Select an option:"
            };

            optionLabel.Draw(canvas);

            _shutdownButton = new ButtonWidget
            {
                X = 10,
                Y = 70,
                Width = 100,
                Height = 30,
                Text = "Shutdown",
                OnClick = () => { Power.Shutdown(); }
            };

            _restartButton = new ButtonWidget
            {
                X = 10,
                Y = 110,
                Width = 100,
                Height = 30,
                Text = "Restart",
                OnClick = () => { Power.Reboot(); }
            };

            _haltButton = new ButtonWidget
            {
                X = 10,
                Y = 150,
                Width = 100,
                Height = 30,
                Text = "Halt",
                OnClick = () => { Power.Halt(); }
            };

            _panicButton = new ButtonWidget
            {
                X = 10,
                Y = 190,
                Width = 100,
                Height = 30,
                Text = "Panic",
                OnClick = () => { PanicHandler.Panic("Intentional panic"); }
            };

            _shutdownButton.Draw(canvas);
            _restartButton.Draw(canvas);
            _haltButton.Draw(canvas);
            _panicButton.Draw(canvas);
        }

        public void HandleInput(int mouseX, int mouseY, bool leftButtonDown)
        {
            _shutdownButton.HandleInput(mouseX, mouseY, leftButtonDown);
            _restartButton.HandleInput(mouseX, mouseY, leftButtonDown);
            _haltButton.HandleInput(mouseX, mouseY, leftButtonDown);
            _panicButton.HandleInput(mouseX, mouseY, leftButtonDown);
        }

        public string WindowTitle(string title)
        {
            return "Power Management";
        }
    }
}

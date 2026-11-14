using Cosmos.Kernel.System.Graphics;
using System;
using Tinos3.Api.Graphical.Widgets;
using Tinos3.Api.Graphical.Windows;

namespace Tinos3.Graphical.Apps.System.Run
{
    public class RunContent : IWindowContent, IWindowInputHandler, IWindowKeyInputHandler
    {
        private readonly Action<string> _executeCommand;
        private readonly ButtonWidget _runButton;

        private readonly TextBox _inputBox = new()
        {
            X = 20,
            Y = 50,
            Width = 300,
            Height = 30,
        };

        public RunContent() : this(_ => { })
        {
        }

        public RunContent(Action<string> executeCommand)
        {
            _executeCommand = executeCommand ?? (_ => { });
            _runButton = new ButtonWidget
            {
                X = 325,
                Y = 50,
                Width = 55,
                Height = 30,
                Text = "Ok",
                OnClick = ExecuteCommand
            };
        }

        public void DrawContent(Canvas canvas)
        {
            _inputBox.Draw(canvas);
            _runButton.Draw(canvas);
        }

        public void HandleInput(int mouseX, int mouseY, bool leftButtonDown)
        {
            _inputBox.HandleInput(mouseX, mouseY, leftButtonDown);
            _runButton.HandleInput(mouseX, mouseY, leftButtonDown);
        }

        public void HandleKeyInput(ConsoleKeyInfo keyInfo)
        {
            if (keyInfo.Key == ConsoleKey.Enter)
            {
                ExecuteCommand();
                return;
            }

            _inputBox.HandleInput(keyInfo);
        }

        private void ExecuteCommand()
        {
            string command = _inputBox.Text.Trim();
            if (command.Length > 0)
            {
                _executeCommand(command);
            }

            _inputBox.SetText(string.Empty);
        }

        public string WindowTitle(string title)
        {
            return "Run";
        }
    }
}

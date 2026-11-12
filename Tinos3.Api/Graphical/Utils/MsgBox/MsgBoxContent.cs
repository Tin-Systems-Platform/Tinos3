using Cosmos.Kernel.System.Graphics;
using System;
using System.Drawing;
using Tinos3.Api.Graphical.Widgets;
using Tinos3.Api.Graphical.Windows;

namespace Tinos3.Api.Graphical.Utils.MsgBox
{
    public class MsgBoxContent : IWindowContent, IWindowInputHandler
    {
        private const int ButtonWidth = 80;
        private const int ButtonHeight = 30;

        public MsgBoxType Type { get; set; }
        public string Message { get; set; }
        public string Title { get; set; }

        private readonly Action<Window> _removeWindow;
        private readonly ButtonWidget _okButton;
        private Window _msgBoxWindow;

        public MsgBoxContent() : this(string.Empty, string.Empty, MsgBoxType.Information, null)
        {
        }

        public MsgBoxContent(string title, string message, MsgBoxType type, Action<Window> removeWindow)
        {
            Title = title ?? string.Empty;
            Message = message ?? string.Empty;
            Type = type;
            _removeWindow = removeWindow;
            _okButton = new ButtonWidget
            {
                Width = ButtonWidth,
                Height = ButtonHeight,
                Text = "OK",
                OnClick = () => _removeWindow?.Invoke(_msgBoxWindow)
            };
        }

        public void SetWindow(Window window)
        {
            _msgBoxWindow = window;
        }

        public void CreateMsgBox()
        {
            DrawContent(Canvas.GetFullScreen());
        }

        public void DrawContent(Canvas canvas)
        {
            new Label
            {
                X = 12,
                Y = 45,
                Text = Message ?? string.Empty
            }.Draw(canvas, Color.Black);

            _okButton.X = (canvas.Width - ButtonWidth) / 2;
            _okButton.Y = canvas.Height - ButtonHeight - 10;
            _okButton.Draw(canvas);
        }

        public void HandleInput(int mouseX, int mouseY, bool leftButtonDown)
        {
            _okButton.HandleInput(mouseX, mouseY, leftButtonDown);
        }

        public string WindowTitle(string title) => Title ?? title;
    }
}

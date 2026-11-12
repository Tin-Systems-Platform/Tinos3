using Cosmos.Kernel.System.Graphics;
using Cosmos.Kernel.System.Graphics.Rendering3D;
using System;
using System.Collections.Generic;
using System.Text;
using Tinos3.Api.Graphical.Widgets;
using Tinos3.Api.Graphical.Windows;

namespace Tinos3.Api.Graphical.Utils.MsgBox
{
    public class MsgBoxContent : IWindowContent, IWindowInputHandler
    {
        public MsgBoxType Type { get; set; }
        public string Message { get; set; }
        public string Title { get; set; }

        private ButtonWidget _okButton;
        private readonly Action<Window> _removeWindow;

        private Window _msgBoxWindow;

        private readonly Action<string> _onOk;
        private readonly Action<string> _onCancel;
        private ButtonWidget _cancelButton;

        private Canvas _canvas = Canvas.GetFullScreen();

        public void CreateMsgBox()
        {
            DrawContent(_canvas);
        }


        public void DrawContent(Canvas canvas)
        {
            ContentPlease(Message, Title, canvas);
        }

        public void HandleInput(int mouseX, int mouseY, bool leftButtonDown)
        {
            _okButton.HandleInput(mouseX, mouseY, leftButtonDown);
            _cancelButton.HandleInput(mouseX, mouseY, leftButtonDown);
        }


        public string WindowTitle(string title)
        {
            title = Title ?? title;
            return title;
        }

        private void ContentPlease(string message, string title, Canvas canvas)
        {
            Label messageLabel = new Label
            {
                X = 10,
                Y = 10,
                Text = message
            };

            _okButton = new ButtonWidget
            {
                X = 0,
                Y = 0,
                Width = 80,
                Height = 30,
                Text = "OK",
                OnClick = () =>
                {
                    _removeWindow?.Invoke(_msgBoxWindow);
                }
            };

            _cancelButton = new ButtonWidget
            {
                X = 0,
                Y = 0,
                Width = 80,
                Height = 30,
                Text = "Cancel",
                OnClick = () =>
                {
                    _removeWindow?.Invoke(_msgBoxWindow);
                }
            };

            if (Type == MsgBoxType.Information || Type == MsgBoxType.Warning || Type == MsgBoxType.Error)
            {
                _okButton.Draw(canvas);
            }
            else if (Type == MsgBoxType.SysError)
            {
                _okButton.Draw(canvas);
                _cancelButton.Draw(canvas);
            }
        }
    }
}

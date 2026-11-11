using Cosmos.Kernel.System.Graphics;
using Cosmos.Kernel.System.Graphics.Fonts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Tinos3.Api.Graphical.Widgets;
using Tinos3.Api.Graphical.Windows;

namespace Tinos3.Graphical.Apps.System.Calc
{
    public class CalcContent : IWindowContent, IWindowInputHandler
    {
        private Label _inputLabel;

        private ButtonWidget _num1;
        private ButtonWidget _num2;

        private  ButtonWidget _num3;
        private  ButtonWidget _num4;
        private  ButtonWidget _num5;
        private  ButtonWidget _num6;
        private  ButtonWidget _num7;
        private  ButtonWidget _num8;
        private  ButtonWidget _num9;
        private  ButtonWidget _num0;
        private  ButtonWidget _add;
        private  ButtonWidget _subtract;
        private  ButtonWidget _multiply;
        private  ButtonWidget _divide;

        private  ButtonWidget _equals;
        private  ButtonWidget _clear;
        private  ButtonWidget _decimal;
        private  ButtonWidget _backspace;

        private CalcContentEngine _contentEngine;
        private CalcEngine _calcEngine;

        public void DrawContent(Canvas canvas)
        {
            _calcEngine = new CalcEngine();
            _contentEngine = new CalcContentEngine();

            _num0 = new ButtonWidget
            {
                X = 20,
                Y = 100,
                Width = 50,
                Height = 50,
                Text = "0",
                OnClick = () => { /* Handle number input */ }
            };

            _num1 = new ButtonWidget
            {
                X = 20,
                Y = 160,
                Width = 50,
                Height = 50,
                Text = "1",
                OnClick = () => { /* Handle number input */ }
            };

            _num2 = new ButtonWidget
            {
                X = 80,
                Y = 160,
                Width = 50,
                Height = 50,
                Text = "2",
                OnClick = () => { /* Handle number input */ }
            };

            _num3 = new ButtonWidget
            {
                X = 140,
                Y = 160,
                Width = 50,
                Height = 50,
                Text = "3",
                OnClick = () => { /* Handle number input */ }
            };

            _num4 = new ButtonWidget
            {
                X = 20,
                Y = 220,
                Width = 50,
                Height = 50,
                Text = "4",
                OnClick = () => { /* Handle number input */ }
            };

            _num5 = new ButtonWidget
            {
                X = 80,
                Y = 220,
                Width = 50,
                Height = 50,
                Text = "5",
                OnClick = () => { /* Handle number input */ }
            };

            _num6 = new ButtonWidget
            {
                X = 140,
                Y = 220,
                Width = 50,
                Height = 50,
                Text = "6",
                OnClick = () => { /* Handle number input */ }
            };

            _num7 = new ButtonWidget
            {
                X = 20,
                Y = 280,
                Width = 50,
                Height = 50,
                Text = "7",
                OnClick = () => { /* Handle number input */ }
            };

            _num8 = new ButtonWidget
            {
                X = 80,
                Y = 280,
                Width = 50,
                Height = 50,
                Text = "8",
                OnClick = () => { /* Handle number input */ }
            };

            _num9 = new ButtonWidget
            {
                X = 140,
                Y = 280,
                Width = 50,
                Height = 50,
                Text = "9",
                OnClick = () => { /* Handle number input */ }
            };

            _subtract = new ButtonWidget
            {
                X = 200,
                Y = 160,
                Width = 50,
                Height = 50,
                Text = "-",
                OnClick = () => { /* Handle subtraction */ }
            };

            _multiply = new ButtonWidget
            {
                X = 200,
                Y = 220,
                Width = 50,
                Height = 50,
                Text = "*",
                OnClick = () => { /* Handle multiplication */ }
            };

            _divide = new ButtonWidget
            {
                X = 200,
                Y = 280,
                Width = 50,
                Height = 50,
                Text = "/",
                OnClick = () => { /* Handle division */ }
            };

            _add = new ButtonWidget
            {
                X = 200,
                Y = 100,
                Width = 50,
                Height = 50,
                Text = "+",
                OnClick = () => { /* Handle addition */ }
            };

            _equals = new ButtonWidget
            {
                X = 260,
                Y = 100,
                Width = 50,
                Height = 50,
                Text = "=",
                OnClick = () => { /* Handle equals */ }
            };

            _clear = new ButtonWidget
            {
                X = 260,
                Y = 160,
                Width = 50,
                Height = 50,
                Text = "C",
                OnClick = () => { /* Handle clear */ }
            };

            _decimal = new ButtonWidget
            {
                X = 260,
                Y = 220,
                Width = 50,
                Height = 50,
                Text = ".",
                OnClick = () => { /* Handle decimal */ }
            };

            _backspace = new ButtonWidget
            {
                X = 260,
                Y = 280,
                Width = 50,
                Height = 50,
                Text = "<-",
                OnClick = () => { /* Handle backspace */ }
            };

            _inputLabel = new Label
            {
                X = 20,
                Y = 40,
                Text = "0",
            };

            // Draw all buttons
            _num0.Draw(canvas);
            _num1.Draw(canvas);
            _num2.Draw(canvas);
            _num3.Draw(canvas);
            _num4.Draw(canvas);
            _num5.Draw(canvas);
            _num6.Draw(canvas);
            _num7.Draw(canvas);
            _num8.Draw(canvas);
            _num9.Draw(canvas);

            _add.Draw(canvas);
            _subtract.Draw(canvas);
            _multiply.Draw(canvas);
            _divide.Draw(canvas);

            _equals.Draw(canvas);
            _clear.Draw(canvas);
            _decimal.Draw(canvas);
            _backspace.Draw(canvas);
            _inputLabel.Draw(canvas);

        }
        public void HandleInput(int mouseX, int mouseY, bool isMouseDown)
        {
            _num0.HandleInput(mouseX, mouseY, isMouseDown);
            _num1.HandleInput(mouseX, mouseY, isMouseDown);
            _num2.HandleInput(mouseX, mouseY, isMouseDown);
            _num3.HandleInput(mouseX, mouseY, isMouseDown);
            _num4.HandleInput(mouseX, mouseY, isMouseDown);
            _num5.HandleInput(mouseX, mouseY, isMouseDown);
            _num6.HandleInput(mouseX, mouseY, isMouseDown);
            _num7.HandleInput(mouseX, mouseY, isMouseDown);
            _num8.HandleInput(mouseX, mouseY, isMouseDown);
            _num9.HandleInput(mouseX, mouseY, isMouseDown);


            _add.HandleInput(mouseX, mouseY, isMouseDown);
            _subtract.HandleInput(mouseX, mouseY, isMouseDown);
            _multiply.HandleInput(mouseX, mouseY, isMouseDown);
            _divide.HandleInput(mouseX, mouseY, isMouseDown);

            _equals.HandleInput(mouseX, mouseY, isMouseDown);
            _clear.HandleInput(mouseX, mouseY, isMouseDown);
            _decimal.HandleInput(mouseX, mouseY, isMouseDown);
            _backspace.HandleInput(mouseX, mouseY, isMouseDown);
        }

        public string WindowTitle(string title)
        {
            return title;
        }
    }
    
    }


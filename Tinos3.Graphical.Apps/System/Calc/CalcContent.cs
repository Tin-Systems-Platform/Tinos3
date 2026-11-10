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
        private readonly ButtonWidget _num1;
        private readonly ButtonWidget _num2;

        private readonly ButtonWidget _num3;
        private readonly ButtonWidget _num4;
        private readonly ButtonWidget _num5;
        private readonly ButtonWidget _num6;
        private readonly ButtonWidget _num7;
        private readonly ButtonWidget _num8;
        private readonly ButtonWidget _num9;
        private readonly ButtonWidget _num0;
        private readonly ButtonWidget _add;
        private readonly ButtonWidget _subtract;
        private readonly ButtonWidget _multiply;
        private readonly ButtonWidget _divide;

        private readonly ButtonWidget _equals;
        private readonly ButtonWidget _clear;
        private readonly ButtonWidget _decimal;
        private readonly ButtonWidget _backspace;


        public void DrawContent(Canvas canvas)
        {
            
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


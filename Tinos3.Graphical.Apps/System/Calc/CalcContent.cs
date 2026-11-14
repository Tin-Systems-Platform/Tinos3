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
        private string _currentInput = "0";
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

        private readonly CalcContentEngine _contentEngine = new CalcContentEngine();
        private readonly CalcEngine _calcEngine = new CalcEngine();
        private float _leftOperand;
        private string _pendingOperator;
        private bool _startNewInput = true;

        public void DrawContent(Canvas canvas)
        {
            if (_num0 == null)
            {
            if (_currentInput == null)
                _currentInput = "0";

            _num0 = new ButtonWidget
            {
                X = 10,
                Y = 100,
                Width = 50,
                Height = 50,
                Text = "0",
                OnClick = () => AddDigit(0)
            };

            _num1 = new ButtonWidget
            {
                X = 10,
                Y = 160,
                Width = 50,
                Height = 50,
                Text = "1",
                OnClick = () => AddDigit(1)
            };

            _num2 = new ButtonWidget
            {
                X = 70,
                Y = 160,
                Width = 50,
                Height = 50,
                Text = "2",
                OnClick = () => AddDigit(2)
            };

            _num3 = new ButtonWidget
            {
                X = 130,
                Y = 160,
                Width = 50,
                Height = 50,
                Text = "3",
                OnClick = () => AddDigit(3)
            };

            _num4 = new ButtonWidget
            {
                X = 10,
                Y = 220,
                Width = 50,
                Height = 50,
                Text = "4",
                OnClick = () => AddDigit(4)
            };

            _num5 = new ButtonWidget
            {
                X = 70,
                Y = 220,
                Width = 50,
                Height = 50,
                Text = "5",
                OnClick = () => AddDigit(5)
            };

            _num6 = new ButtonWidget
            {
                X = 130,
                Y = 220,
                Width = 50,
                Height = 50,
                Text = "6",
                OnClick = () => AddDigit(6)
            };

            _num7 = new ButtonWidget
            {
                X = 10,
                Y = 280,
                Width = 50,
                Height = 50,
                Text = "7",
                OnClick = () => AddDigit(7)
            };

            _num8 = new ButtonWidget
            {
                X = 70,
                Y = 280,
                Width = 50,
                Height = 50,
                Text = "8",
                OnClick = () => AddDigit(8)
            };

            _num9 = new ButtonWidget
            {
                X = 130,
                Y = 280,
                Width = 50,
                Height = 50,
                Text = "9",
                OnClick = () => AddDigit(9)
            };

            _subtract = new ButtonWidget
            {
                X = 190,
                Y = 160,
                Width = 50,
                Height = 50,
                Text = "-",
                OnClick = () => SetOperator("-")
            };

            _multiply = new ButtonWidget
            {
                X = 190,
                Y = 220,
                Width = 50,
                Height = 50,
                Text = "*",
                OnClick = () => SetOperator("*")
            };

            _divide = new ButtonWidget
            {
                X = 190,
                Y = 280,
                Width = 50,
                Height = 50,
                Text = "/",
                OnClick = () => SetOperator("/")
            };

            _add = new ButtonWidget
            {
                X = 190,
                Y = 100,
                Width = 50,
                Height = 50,
                Text = "+",
                OnClick = () => SetOperator("+")
            };

            _equals = new ButtonWidget
            {
                X = 250,
                Y = 100,
                Width = 50,
                Height = 50,
                Text = "=",
                OnClick = Calculate
            };

            _clear = new ButtonWidget
            {
                X = 250,
                Y = 160,
                Width = 50,
                Height = 50,
                Text = "C",
                OnClick = Clear
            };

            _decimal = new ButtonWidget
            {
                X = 250,
                Y = 220,
                Width = 50,
                Height = 50,
                Text = ".",
                OnClick = AddDecimal
            };

            _backspace = new ButtonWidget
            {
                X = 250,
                Y = 280,
                Width = 50,
                Height = 50,
                Text = "<-",
                OnClick = Backspace
            };

            _inputLabel = new Label
            {
                X = 20,
                Y = 40,
                Text = _currentInput,
            };
            }

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

        private void AddDigit(int digit)
        {
            if (_startNewInput || string.IsNullOrEmpty(_currentInput) || _currentInput == "0")
                _currentInput = _contentEngine.ClearString();

            _currentInput = _contentEngine.AddNumToString(_currentInput, digit);
            _startNewInput = false;
            UpdateDisplay();
        }

        private void AddDecimal()
        {
            if (_startNewInput)
            {
                _currentInput = _contentEngine.ClearString();
                _startNewInput = false;
            }
            _currentInput = _contentEngine.AppendDecimal(_currentInput);
            UpdateDisplay();
        }

        private void Backspace()
        {
            if (_startNewInput)
                return;
            _currentInput = _contentEngine.RemoveLastCharFromString(_currentInput);
            if (string.IsNullOrEmpty(_currentInput))
                _currentInput = "0";
            UpdateDisplay();
        }

        private void SetOperator(string op)
        {
            if (!float.TryParse(_currentInput, out _leftOperand))
                return;
            _pendingOperator = op;
            _startNewInput = true;
        }

        private void Calculate()
        {
            if (string.IsNullOrEmpty(_pendingOperator) || !float.TryParse(_currentInput, out float rightOperand))
                return;

            try
            {
                float result;
                switch (_pendingOperator)
                {
                    case "+": result = _calcEngine.Add(_leftOperand, rightOperand); break;
                    case "-": result = _calcEngine.Subtract(_leftOperand, rightOperand); break;
                    case "*": result = _calcEngine.Multiply(_leftOperand, rightOperand); break;
                    case "/": result = _calcEngine.Divide(_leftOperand, rightOperand); break;
                    default: return;
                }
                _currentInput = result.ToString();
                _pendingOperator = null;
                _startNewInput = true;
            }
            catch (DivideByZeroException)
            {
                _currentInput = "Error";
                _pendingOperator = null;
                _startNewInput = true;
            }
            UpdateDisplay();
        }

        private void Clear()
        {
            _currentInput = _contentEngine.ClearString();
            _leftOperand = 0;
            _pendingOperator = null;
            _startNewInput = true;
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            if (_inputLabel != null)
                _inputLabel.Text = string.IsNullOrEmpty(_currentInput)
                    ? "0"
                    : _contentEngine.getCurrentInput(_currentInput);
        }

        public string WindowTitle(string title)
        {
            return title;
        }
    }
    
    }


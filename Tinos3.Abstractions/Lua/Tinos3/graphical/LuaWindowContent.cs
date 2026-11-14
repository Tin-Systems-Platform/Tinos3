using Cosmos.Kernel.System.Graphics;
using System.Drawing;
using Tinos3.Api.Graphical.Windows;
using Tinos3.Api.Graphical.Widgets;

namespace Tinos3.Abstractions.Lua.Tinos3.graphical;

internal sealed class LuaWindowContent : IWindowContent, IWindowInputHandler, IWindowKeyInputHandler
{
    private readonly List<(int X, int Y, string Text)> _labels = new();
    private readonly List<ButtonWidget> _buttons = new();
    private readonly List<TextBox> _textBoxes = new();

    public string WindowTitle(string title) => title;

    public void AddLabel(int x, int y, string text)
    {
        _labels.Add((x, y, text));
    }

    public void AddButton(ButtonWidget button)
    {
        _buttons.Add(button);
    }

    public int AddTextBox(TextBox textBox)
    {
        _textBoxes.Add(textBox);
        return _textBoxes.Count;
    }

    public bool TryGetTextBox(int id, out TextBox textBox)
    {
        if (id > 0 && id <= _textBoxes.Count)
        {
            textBox = _textBoxes[id - 1];
            return true;
        }

        textBox = null!;
        return false;
    }

    public void DrawContent(Canvas canvas)
    {
        foreach (var label in _labels)
        {
            GraphicalFont.DrawString(canvas, label.Text, Color.Black, label.X, label.Y);
        }

        foreach (ButtonWidget button in _buttons)
        {
            button.Draw(canvas);
        }

        foreach (TextBox textBox in _textBoxes)
        {
            textBox.Draw(canvas);
        }
    }

    public void HandleInput(int mouseX, int mouseY, bool leftButtonDown)
    {
        foreach (ButtonWidget button in _buttons)
        {
            button.HandleInput(mouseX, mouseY, leftButtonDown);
        }

        foreach (TextBox textBox in _textBoxes)
        {
            textBox.HandleInput(mouseX, mouseY, leftButtonDown);
        }
    }

    public void HandleKeyInput(ConsoleKeyInfo keyInfo)
    {
        foreach (TextBox textBox in _textBoxes)
        {
            textBox.HandleInput(keyInfo);
        }
    }
}

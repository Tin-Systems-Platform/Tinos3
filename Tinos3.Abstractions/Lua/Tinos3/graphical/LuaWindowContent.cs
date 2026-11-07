using Cosmos.Kernel.System.Graphics;
using Cosmos.Kernel.System.Graphics.Fonts;
using System.Drawing;
using Tinos3.Api.Graphical.Windows;
using Tinos3.Api.Graphical.Widgets;

namespace Tinos3.Abstractions.Lua.Tinos3.graphical;

internal sealed class LuaWindowContent : IWindowContent, IWindowInputHandler
{
    private readonly List<(int X, int Y, string Text)> _labels = new();
    private readonly List<ButtonWidget> _buttons = new();

    public string WindowTitle(string title) => title;

    public void AddLabel(int x, int y, string text)
    {
        _labels.Add((x, y, text));
    }

    public void AddButton(ButtonWidget button)
    {
        _buttons.Add(button);
    }

    public void DrawContent(Canvas canvas)
    {
        foreach (var label in _labels)
        {
            canvas.DrawString(label.Text, PCScreenFont.DefaultFont, Color.Black, label.X, label.Y);
        }

        foreach (ButtonWidget button in _buttons)
        {
            button.Draw(canvas);
        }
    }

    public void HandleInput(int mouseX, int mouseY, bool leftButtonDown)
    {
        foreach (ButtonWidget button in _buttons)
        {
            button.HandleInput(mouseX, mouseY, leftButtonDown);
        }
    }
}

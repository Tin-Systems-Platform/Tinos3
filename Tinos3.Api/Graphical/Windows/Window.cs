using System;
using Cosmos.Kernel.System;
using Cosmos.Kernel.System.Graphics;
using System.Drawing;
using Tinos3.Api.Graphical.Widgets;

namespace Tinos3.Api.Graphical.Windows;

public class Window
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public string Title { get; set; }
    public bool IsActive { get; set; }

    // Efficiency: Only redraw the off-screen canvas when something changes
    public bool IsDirty { get; set; } = true;

    private readonly Canvas _offscreenCanvas;
    private readonly ButtonWidget _closeButton;

    private readonly IWindowContent _content;

    public Window(int x, int y, int width, int height, string title, IWindowContent windowContent = null)
    {
        X = x; Y = y; Width = width; Height = height; Title = title;
        _offscreenCanvas = new Canvas(width, height);
        _content = windowContent;

        // Initialize Close Button (top right of title bar)
        _closeButton = new ButtonWidget
        {
            X = width - 30,
            Y = 4,
            Width = 26,
            Height = 26,
            Text = "X",
            OnClick = () => { /* Handled by WindowManager for removal */ }
        };
    }
    public void DrawContent()
    {
        // Clear background
        _offscreenCanvas.Clear(IsActive ? Color.LightGray : Color.Silver);

        // Title Bar
        Color titleBarColor = IsActive ? Color.DarkBlue : Color.Gray;
        _offscreenCanvas.DrawFilledRectangle(titleBarColor, 0, 0, Width, 30);
        _offscreenCanvas.DrawLine(Color.Black, 0, 30, Width, 30);

        // Window Border
        _offscreenCanvas.DrawRectangle(Color.Black, 0, 0, Width, Height);

        // Title Text
        GraphicalFont.DrawString(_offscreenCanvas, Title, Color.White, 5, 3);

        // Draw Close Button
        _closeButton.Draw(_offscreenCanvas);

        //TODO: Add support for adding widgets managed by the application to the window and render them here. For now, we can just draw some example content.
        // Example Content
        _content?.DrawContent(_offscreenCanvas);

        IsDirty = false; // Mark as clean after drawing
    }

    public void Render(Canvas targetCanvas)
    {
        if (IsDirty)
        {
            DrawContent();
        }
        // Blit the pre-rendered window to the main canvas
        targetCanvas.DrawCanvas(_offscreenCanvas, X, Y);
    }

    public bool ContainsPoint(int x, int y)
    {
        return x >= X && x <= X + Width && y >= Y && y <= Y + Height;
    }

    public bool IsInTitleBar(int x, int y)
    {
        return ContainsPoint(x, y) && y <= Y + 25 && x <= X + Width - 25; // Exclude close button area
    }

    public bool IsCloseButtonHovered(int x, int y)
    {
        return _closeButton.IsHovered(x - X, y - Y);
    }

    public void HandleContentInput(int mouseX, int mouseY, bool leftButtonDown)
    {
        if (_content is IWindowInputHandler inputHandler)
        {
            inputHandler.HandleInput(mouseX - X, mouseY - Y, leftButtonDown);
            IsDirty = true;
        }
    }

    public void HandleContentKeyInput(ConsoleKeyInfo keyInfo)
    {
        if (_content is IWindowKeyInputHandler keyInputHandler)
        {
            keyInputHandler.HandleKeyInput(keyInfo);
            IsDirty = true;
        }
    }

    public void WindowTitle(string title)
    {
        Title = title;
        IsDirty = true;
    }

    // Blank constructor for cases where you want to use
    // the Window class without immediately setting properties or at all.
    public Window() { }
}

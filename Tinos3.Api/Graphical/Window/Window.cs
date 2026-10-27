using System;
using Cosmos.Kernel.System;
using Cosmos.Kernel.System.Graphics;
using Cosmos.Kernel.System.Graphics.Fonts;
using System.Drawing;
using Tinos3.Api.Graphical.Widgets;

namespace Tinos3.Api.Graphical.Window;

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

    public Window(int x, int y, int width, int height, string title)
    {
        X = x; Y = y; Width = width; Height = height; Title = title;
        _offscreenCanvas = new Canvas(width, height);

        // Initialize Close Button (top right of title bar)
        _closeButton = new ButtonWidget
        {
            X = width - 22,
            Y = 2,
            Width = 18,
            Height = 18,
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
        _offscreenCanvas.DrawFilledRectangle(titleBarColor, 0, 0, Width, 25);
        _offscreenCanvas.DrawLine(Color.Black, 0, 25, Width, 25);

        // Window Border
        _offscreenCanvas.DrawRectangle(Color.Black, 0, 0, Width, Height);

        // Title Text
        _offscreenCanvas.DrawString(Title, PCScreenFont.DefaultFont, Color.White, 5, 5);

        // Draw Close Button
        _closeButton.Draw(_offscreenCanvas, false, false);

        // Example Content
        _offscreenCanvas.DrawString("Hello from Cosmos OS!", PCScreenFont.DefaultFont, Color.Black, 10, 40);
        _offscreenCanvas.DrawString("Efficient dirty-rect rendering.", PCScreenFont.DefaultFont, Color.Black, 10, 60);

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
}

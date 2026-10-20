using Cosmos.Kernel.System.Graphics;
using System.Drawing;

namespace Tinos3.Graphical.Desktop;

public sealed class CanvasDesktopRenderer : IDesktopRenderer
{
    private readonly Canvas canvas;
    private DesktopSize frameSize;

    public CanvasDesktopRenderer(Canvas canvas)
    {
        this.canvas = canvas ?? throw new ArgumentNullException(nameof(canvas));
    }

    public void BeginFrame(DesktopSize size)
    {
        frameSize = size;
        canvas.Clear(Color.DarkBlue);
    }

    public void RenderWindow(DesktopWindow window)
    {
        var left = Math.Max(0, window.Position.X);
        var top = Math.Max(0, window.Position.Y);
        var right = Math.Min(frameSize.Width - 1, window.Position.X + window.Size.Width - 1);
        var bottom = Math.Min(frameSize.Height - 1, window.Position.Y + window.Size.Height - 1);

        if (left > right || top > bottom)
        {
            return;
        }

        var border = window.IsFocused ? Color.White : Color.LightGray;
        canvas.DrawRectangle(Color.Gray, left, top, right - left + 1, bottom - top + 1);
        canvas.DrawLine(border, left, top, right, top);
        canvas.DrawLine(border, left, bottom, right, bottom);
        canvas.DrawLine(border, left, top, left, bottom);
        canvas.DrawLine(border, right, top, right, bottom);
    }

    public void EndFrame()
    {
        canvas.Display();
    }
}

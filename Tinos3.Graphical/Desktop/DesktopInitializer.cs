using Cosmos.Kernel.System.Graphics;

namespace Tinos3.Graphical.Desktop;

public static class DesktopInitializer
{
    public static Desktop Initialize(Canvas canvas)
    {
        ArgumentNullException.ThrowIfNull(canvas);

        var renderer = new CanvasDesktopRenderer(canvas);
        return Initialize(
            new DesktopSize(canvas.Width, canvas.Height),
            renderer);
    }

    public static Desktop Initialize(DesktopSize size, IDesktopRenderer renderer)
    {
        ArgumentNullException.ThrowIfNull(renderer);

        var desktop = new Desktop(size);
        desktop.CreateWindow(
            "Tinos3 Desktop",
            new DesktopPoint(2, 2),
            new DesktopSize(Math.Min(40, size.Width), Math.Min(10, size.Height)));
        desktop.Render(renderer);
        return desktop;
    }

    public static Desktop Initialize()
    {
        var canvas = Canvas.GetFullScreen();
        return Initialize(canvas);
    }
}

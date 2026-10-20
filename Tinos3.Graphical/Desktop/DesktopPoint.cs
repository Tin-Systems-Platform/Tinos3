namespace Tinos3.Graphical.Desktop;

public readonly struct DesktopPoint
{
    public DesktopPoint(int x, int y)
    {
        X = x;
        Y = y;
    }

    public int X { get; }

    public int Y { get; }
}

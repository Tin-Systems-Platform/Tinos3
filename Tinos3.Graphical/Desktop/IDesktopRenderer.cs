namespace Tinos3.Graphical.Desktop;

public interface IDesktopRenderer
{
    void BeginFrame(DesktopSize size);

    void RenderWindow(DesktopWindow window);

    void EndFrame();
}

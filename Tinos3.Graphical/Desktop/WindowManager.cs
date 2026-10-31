using Cosmos.Kernel.System.Graphics;
using Cosmos.Kernel.System.Graphics.Fonts;
using Cosmos.Kernel.System.Input;
using System;
using System.Drawing;
using Tinos3.Api.Graphical;
using Tinos3.Api.Graphical.Window;

namespace Tinos3.Graphical.Desktop;

public class WindowManager
{
    private readonly List<Window> _windows;

    // Edge-triggered mouse state
    private bool _wasLeftButtonDown;
    private bool _isDragging;
    private int _dragOffsetX;
    private int _dragOffsetY;
    private Window _draggedWindow;
    private GetGraphicsResources _graphicsResources;

    public int ScreenWidth { get; set; }
    public int ScreenHeight { get; set; }

    public WindowManager(int screenWidth, int screenHeight)
    {
        _windows = new List<Window>();
        ScreenWidth = screenWidth;
        ScreenHeight = screenHeight;
        _graphicsResources = new GetGraphicsResources();
    }

    public void AddWindow(Window window)
    {
        _windows.Add(window);
        SetActiveWindow(window);
    }

    public void RemoveWindow(Window window)
    {
        _windows.Remove(window);
        // Mark all remaining windows as dirty since the background behind them changed
        foreach (var w in _windows) w.IsDirty = true;
    }

    public void SetActiveWindow(Window window)
    {
        foreach (var w in _windows)
        {
            if (w.IsActive != (w == window))
            {
                w.IsActive = (w == window);
                w.IsDirty = true; // Focus change requires redraw
            }
        }
        // Move to end of list for correct Z-ordering (renders last = on top)
        _windows.Remove(window);
        _windows.Add(window);
    }

    public void HandleInput()
    {
        int mouseX = (int)MouseManager.X;
        int mouseY = (int)MouseManager.Y;
        bool isLeftButtonDown = MouseManager.LeftButton;

        // 1. Detect Mouse Down (Rising Edge)
        if (isLeftButtonDown && !_wasLeftButtonDown)
        {
            // Check windows from top to bottom
            for (int i = _windows.Count - 1; i >= 0; i--)
            {
                Window w = _windows[i];

                // Check Close Button Click
                if (w.IsActive && w.IsCloseButtonHovered(mouseX, mouseY))
                {
                    RemoveWindow(w);
                    break; // Stop processing this click
                }

                if (w.IsInTitleBar(mouseX, mouseY))
                {
                    SetActiveWindow(w);
                    _isDragging = true;
                    _draggedWindow = w;
                    _dragOffsetX = mouseX - w.X;
                    _dragOffsetY = mouseY - w.Y;
                    break;
                }
                else if (w.ContainsPoint(mouseX, mouseY))
                {
                    SetActiveWindow(w);
                    break;
                }
            }
        }

        // 2. Stop dragging immediately when the left button is released.
        if (!isLeftButtonDown)
        {
            _isDragging = false;
            _draggedWindow = null;
        }

        // 3. Handle Dragging only while the left button is held.
        if (isLeftButtonDown && _isDragging && _draggedWindow != null)
        {
            _draggedWindow.X = mouseX - _dragOffsetX;
            _draggedWindow.Y = mouseY - _dragOffsetY;

            // Clamp to screen bounds (Architecture/Robustness)
            if (_draggedWindow.X < 0) _draggedWindow.X = 0;
            if (_draggedWindow.Y < 0) _draggedWindow.Y = 0;
            if (_draggedWindow.X + _draggedWindow.Width > ScreenWidth)
                _draggedWindow.X = ScreenWidth - _draggedWindow.Width;
            if (_draggedWindow.Y + _draggedWindow.Height > ScreenHeight)
                _draggedWindow.Y = ScreenHeight - _draggedWindow.Height;

            _draggedWindow.IsDirty = true; // Movement requires redraw
        }

        _wasLeftButtonDown = isLeftButtonDown;
    }

    public void RenderAll(Canvas targetCanvas)
    {
        foreach (var window in _windows)
        {
            window.Render(targetCanvas);
        }
    }
}

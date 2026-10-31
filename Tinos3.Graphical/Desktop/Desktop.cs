using Cosmos.Kernel.System.Diagnostics;
using Cosmos.Kernel.System.Graphics;
using Cosmos.Kernel.System.Graphics.Fonts;
using Cosmos.Kernel.System.Graphics.Rendering3D;
using Cosmos.Kernel.System.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Tinos3.Api.Graphical;
using Tinos3.Api.Graphical.Window;
using Tinos3.Api.Internal;

namespace Tinos3.Graphical.Desktop
{
    public class Desktop
    {
        private Canvas _mainCanvas;
        private Canvas _desktopCanvas; // Pre-rendered background for efficiency
        private WindowManager _windowManager;
        private byte[] _cursorImage;
        private Png _cursor;
        private Font _font = PCScreenFont.DefaultFont;
        private TaskBar _taskBar = new TaskBar();

        public void initDesktop()
        {
            _mainCanvas = Canvas.GetFullScreen();
            MouseManager.SetScreenSize(_mainCanvas.Width, _mainCanvas.Height);

            // Decode Graphics Resources (like cursor image) from embedded resources
            var bytes = GetGraphicsResources.Get("Tinos3.Resources.Graphical.MouseCursor.png");

            if (File.Exists("/mnt/cursor.png"))
            {
                _cursorImage = File.ReadAllBytes("/mnt/cursor.png");
                _cursor = new Png("/mnt/cursor.png");
            }
            else
            {
                _cursorImage = bytes;
                File.WriteAllBytes("/mnt/cursor.png", _cursorImage);
                _cursor = new Png("/mnt/cursor.png");
            }

            // EFFICIENCY: Pre-render the desktop background once
            _desktopCanvas = new Canvas(_mainCanvas.Width, _mainCanvas.Height);
            DrawDesktopBackground(_desktopCanvas);

            _windowManager = new WindowManager(_mainCanvas.Width, _mainCanvas.Height);

            // Create sample windows
            _windowManager.AddWindow(new Window(50, 50, 320, 200, "Window 1"));
            _windowManager.AddWindow(new Window(150, 150, 350, 250, "Window 2"));

        }

        private void DrawDesktopBackground(Canvas canvas)
        {
            canvas.Clear(Color.Navy);
            // Draw grid once
            for (int i = 0; i < canvas.Width; i += 40)
                canvas.DrawLine(Color.Blue, i, 0, i, canvas.Height);
            for (int i = 0; i < canvas.Height; i += 40)
                canvas.DrawLine(Color.Blue, 0, i, canvas.Width, i);
        }

        public void ActiveRenderLoop()
        {
            while (true)
            {
                // 1. Handle Input (updates window positions and dirty flags)
                _windowManager.HandleInput();

                // 2. EFFICIENCY: Blit pre-rendered background instead of redrawing primitives
                _mainCanvas.DrawCanvas(_desktopCanvas, 0, 0);

                // 3. Render windows (only draws off-screen canvas if IsDirty == true)
                _windowManager.RenderAll(_mainCanvas);
                _taskBar.TaskBarRenderer(_desktopCanvas);

                // 4. Draw mouse cursor
                int mx = (int)MouseManager.X;
                int my = (int)MouseManager.Y;

                _mainCanvas.DrawImage(_cursor, mx, my, _cursor.Height/4, _cursor.Width/4);

                // 5. Present frame (Gen3 requires this for double-buffering)
                _mainCanvas.Display();

                // ReadKey blocks until another key arrives, which would stop the
                // desktop from presenting mouse movement between key presses.
                if (KeyboardManager.TryReadKey(out KeyEvent? keyEvent)
                    && keyEvent.Key == Key.Escape)
                {
                    break;
                }

                // Small yield to prevent 100% CPU lock in QEMU/VMware
                System.Threading.Thread.Sleep(10);
            }

        }

    }
}

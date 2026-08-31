using Cosmos.Kernel.System.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Cosmos.Kernel.System.Mouse;
using Cosmos.Kernel.System.Graphics.Fonts;
using Cosmos.Kernel.System.Keyboard;
using Cosmos.Kernel.System.Keyboard.ScanMaps;

namespace Tinos3.Graphics.Desktop
{
    public class InitDesktop
    {
        internal void DesktopInit(Boolean dontClear, Boolean dontLog)
        {
            Bitmap StartButton = new Bitmap(3, 1, new byte[]
        {
               0, 0, 0, 255,
               0, 0, 0, 255,
               0, 0, 0, 255
        }, ColorDepth.ColorDepth32);

            if (dontClear && dontLog) {
                Canvas canvas = Canvas.GetFullScreen();


                KeyEvent key = KeyboardManager.ReadKey();

                int x = (canvas.Width - 60) / 2;
                int y = (canvas.Height - 60) / 2;

                /* Clamp the pointer to the actual screen */
                MouseManager.SetScreenSize(canvas.Width, canvas.Height);


                canvas.DrawImage(StartButton, x, y);

                canvas.Display();
            } else
            {
                Console.WriteLine("GRAPHICS: Desktop initializing");

                Canvas canvas = Canvas.GetFullScreen();
                DesktopBitmaps desktopBitmaps = new DesktopBitmaps();


                canvas.Clear(Color.DarkBlue);

                canvas.DrawImage(StartButton, 0, 0);

                canvas.Display(); 
            }
        }

        public void ShowDesktop(Boolean indefinetly)
        {
            Canvas canvas = Canvas.GetFullScreen();
            
            canvas.Clear(Color.DarkBlue);

            if (indefinetly)
            {
                while (true)
                {

                    KeyEvent key = KeyboardManager.ReadKey();

                    if (key.Key == ConsoleKeyEx.Escape)
                    {
                        Console.Clear();
                        Console.WriteLine("Exiting Desktop");
                        break;
                    }
                    DesktopInit(true, true);
                }
            } else
            {
                Console.WriteLine("Not showing desktop indefinetly");
                DesktopInit(true, true);
            }

        }
    }
}

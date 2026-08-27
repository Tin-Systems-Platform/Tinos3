using Cosmos.Kernel.System.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Tinos3.Graphics.Desktop
{
    public class InitDesktop
    {
        internal void DesktopInit(Boolean dontClear, Boolean dontLog)
        {
            Bitmap StartButton = new Bitmap(3, 1, new byte[]
        {
            0,0,0, 255
        }, ColorDepth.ColorDepth32);

            if (dontClear && dontLog) {
                Canvas canvas = Canvas.GetFullScreen();

                canvas.DrawImage(StartButton, 0, 0);

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

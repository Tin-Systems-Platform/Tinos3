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
            if (dontClear && dontLog) {
                Canvas canvas = Canvas.GetFullScreen();

                canvas.Display();
            } else
            {
                Console.WriteLine("GRAPHICS: Desktop initializing");

                Canvas canvas = Canvas.GetFullScreen();

                canvas.Clear(Color.DarkBlue);

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

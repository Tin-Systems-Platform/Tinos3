using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using Cosmos.Kernel.System.Graphics;
using Cosmos.Kernel.System.Graphics.Fonts;

namespace Tinos3.Graphics
{
    public class DisplayDetection
    {
        public void InitialDisplayDetection()
        {
            Canvas canvas = Canvas.GetFullScreen();

            Console.WriteLine("Display stats detected: ");

            Console.WriteLine("Canvas:     " + canvas.Name());
            Console.WriteLine("Resolution: " + canvas.Width + "x" + canvas.Height);
            Console.WriteLine("Refresh:    " + canvas.RefreshRate + " Hz");
        }
    }
}

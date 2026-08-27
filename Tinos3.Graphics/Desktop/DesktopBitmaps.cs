using Cosmos.Kernel.System.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tinos3.Graphics.Desktop
{
    internal class DesktopBitmaps
    {
        private Bitmap startButton = new Bitmap(3, 1, new byte[] 
        { 
            0,0,0, 255
        }, ColorDepth.ColorDepth32);

        public Bitmap StartButton {
            get => startButton;
            set => startButton = value;
        }
    }
}

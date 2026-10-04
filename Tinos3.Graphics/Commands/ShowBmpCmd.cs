using Cosmos.Kernel.System.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Tinos3.Abstractions.Commands;

namespace Tinos3.Graphics.Commands
{
    public class ShowBmpCmd : Command
    {
        public ShowBmpCmd(string name) : base(name)
        {
        }

        public override string Execute(string[] args)
        {

            Canvas canvas = Canvas.GetFullScreen();

            Bitmap logo = new Bitmap(@"/mnt/tinos3_test_bmp.bmp");

            canvas.DrawImage(logo,
                (canvas.Width - (int)logo.Width) / 2,
                (canvas.Height - (int)logo.Height) / 2);

            return "";
        }
    }
}

using Cosmos.Kernel.System.Graphics;
using Cosmos.Kernel.System.Graphics.Fonts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Tinos3.Abstractions.Commands;

namespace Tinos3.Graphics.Commands
{
    public class ShapesCmd : Command
    {
        public ShapesCmd(string name) : base(name)
        {
        }

        public override string Execute(string[] args)
        {
            Canvas canvas = Canvas.GetFullScreen();



            /* A single point */
            canvas.DrawPoint(Color.White, 100, 100);

            /* Lines: horizontal, vertical and diagonal */
            canvas.DrawLine(Color.GreenYellow, 250, 100, 400, 100);
            canvas.DrawLine(Color.IndianRed, 350, 150, 350, 250);
            canvas.DrawLine(Color.MintCream, 250, 150, 400, 250);

            /* Outlined and filled rectangles */
            canvas.DrawRectangle(Color.PaleVioletRed, 450, 100, 120, 80);
            canvas.DrawFilledRectangle(Color.Chocolate, 450, 220, 120, 80);

            /* Circles and ellipses */
            canvas.DrawCircle(Color.Chartreuse, 130, 200, 40);
            canvas.DrawFilledCircle(Color.MediumOrchid, 130, 320, 40);
            canvas.DrawEllipse(Color.DeepSkyBlue, 300, 350, 60, 30);

            /* An arc: angles are in degrees */
            canvas.DrawArc(500, 400, 50, 50, Color.CadetBlue, 90, 270);

            /* Triangles and polygons */
            canvas.DrawTriangle(Color.Gold, 600, 100, 650, 200, 550, 200);
            canvas.DrawPolygon(Color.MediumPurple,
                new Point(650, 300), new Point(720, 340), new Point(700, 420), new Point(620, 400));

            Font font = PCScreenFont.DefaultFont;
            canvas.DrawString("Hello Cosmos World!", font, Color.White, 100, 100);
            canvas.DrawString("Spleen " + font.Width + "x" + font.Height + " (built in)",
                font, Color.GreenYellow, 100, 140);


            Bitmap bitmap = new Bitmap(2, 2, new byte[]
{
    255, 0, 0, 255,      // blue
    0, 255, 0, 255,      // green
    0, 0, 255, 255,      // red
    255, 255, 255, 255,  // white
}, ColorDepth.ColorDepth32);

            /* Draw it pixel-for-pixel, then scaled up to 128x128 */
            canvas.DrawImage(bitmap, 100, 100);
            canvas.DrawImage(bitmap, 100, 150, 128, 128);

            /* Swap the finished frame to the screen */
            canvas.Display();

            Thread.Sleep(10000);
            canvas.Clear();

            return "";
        }
    }
}

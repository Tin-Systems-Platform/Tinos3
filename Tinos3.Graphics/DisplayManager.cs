using Cosmos.Kernel.System.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Tinos3.Graphics
{
    public class DisplayManager
    {
        private Canvas _canvas;

        public void InitializeStaticResolution()
        {
            // 1. Instantiate the target resolution directly 
            // In Gen 3, Mode properties are Width, Height, and ColorDepth
            Mode targetMode = new Mode(1920, 1080, ColorDepth.ColorDepth32);

            // 2. Fetch the safe read-only collection of modes
            // Since Mode is structural, we can check capability right away
            IReadOnlyList<Mode> hardwareModes = Canvas.GetFullScreen().AvailableModes;
            bool modeFound = false;

            foreach (Mode mode in hardwareModes)
            {
                // Replaced 'Columns' and 'Rows' with 'Width' and 'Height'
                if (mode.Width == targetMode.Width &&
                    mode.Height == targetMode.Height &&
                    mode.ColorDepth == targetMode.ColorDepth)
                {
                    modeFound = true;
                    break;
                }
            }

            // 3. Fallback logic if 1080p is unavailable in the boot config
            if (!modeFound)
            {
                targetMode = new Mode(1024, 768, ColorDepth.ColorDepth32);
            }

            // 4. Initialize the final canvas by passing the targetMode directly
            // This satisfies the read-only Mode constraint in Gen 3
            _canvas = Canvas.GetFullScreen(targetMode);

            // Clean the background
            _canvas.Clear(Color.Black);
        }
    }
}

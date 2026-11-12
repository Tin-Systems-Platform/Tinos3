using Cosmos.Kernel.System.Graphics;
using Cosmos.Kernel.System.Graphics.Fonts;
using System;
using System.Drawing;
using System.IO;

namespace Tinos3.Api.Graphical.Widgets;

/// <summary>Shared custom font used by graphical text throughout Tinos3.</summary>
public static class GraphicalFont
{
    public const int DefaultSize = 18;
    private static TrueTypeFont _font = null!;

    public static TrueTypeFont Font
    {
        get
        {
            if (_font == null)
            {
                if (!File.Exists("/mnt/font.ttf"))
                {
                    try
                    {
                        File.Copy("/cd/font.ttf", "/mnt/font.ttf");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error copying font file: {ex.Message}");
                        throw;
                    }
                }

                _font = new TrueTypeFont("/mnt/font.ttf");
            }

            return _font;
        }
    }

    public static void DrawString(Canvas canvas, string text, Color color, int x, int y)
    {
        if (!string.IsNullOrEmpty(text))
        {
            canvas.DrawString(text, Font, DefaultSize, color, x, y);
        }
    }

    public static int MeasureString(string text)
    {
        return string.IsNullOrEmpty(text) ? 0 : Font.MeasureString(text, DefaultSize);
    }
}

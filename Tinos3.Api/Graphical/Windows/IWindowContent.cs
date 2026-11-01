using Cosmos.Kernel.System.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tinos3.Api.Graphical.Windows
{
    /// <summary>
    /// Interface for window content. This can be implemented by applications to provide custom content for windows.
    /// Displayed content can be drawn in the DrawContent method of the Window class, which can call methods on the IWindowContent implementation to render specific content.
    /// </summary>
    public interface IWindowContent
    {
        /// <summary>
        /// Draws the content of the window. This method is called by the Window class when rendering the window.
        /// </summary>
        /// <param name="canvas">The canvas to draw on.</param>
        void DrawContent(Canvas canvas);

        /// <summary>
        /// Manages the window title. This method can be called to update the window's title based on the content or state of the application.
        /// </summary>
        /// <param name="title">The window title.</param>
        string WindowTitle(string title);

        /// <summary>
        /// Optional. Allows the application to set a custom icon for the window. This can be used to provide a visual representation of the application or its state.
        /// It is set to null by default, indicating that no custom icon is provided. If an application wants to provide a custom icon, it can implement this property and return the path to the icon file.
        /// WindowManager can then use this icon when rendering the window, for example, in the title bar or taskbar.
        /// </summary>
        string? IconPath => null;

    }
}

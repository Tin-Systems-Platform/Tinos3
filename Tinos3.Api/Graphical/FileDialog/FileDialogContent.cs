using Cosmos.Kernel.System.Graphics;
using Cosmos.Kernel.System.Input;
using System;
using System.Collections.Generic;
using System.Text;
using Tinos3.Api.Graphical.Widgets;
using Tinos3.Api.Graphical.Windows;
using System.IO;
using Tinos3.Api.Utilities;

namespace Tinos3.Api.Graphical.FileDialog
{
    public class FileDialogContent : IWindowContent, IWindowInputHandler
    {
        private string _currentPath = "/";
        private string _selectedFile = string.Empty;
        private List<string> _filesAndDirectoriesInDirectory = new List<string>();

        public string CurrentPath
        {
            get => _currentPath;
            set => _currentPath = value;
        }

        public string SelectedFile
        {
            get => _selectedFile;
            set => _selectedFile = value;
        }

        public List<string> FilesAndDirectoriesInDirectory
        {
            get => _filesAndDirectoriesInDirectory;
            set => _filesAndDirectoriesInDirectory = value;
        }

        private ButtonWidget _openButton;

        private ButtonWidget _cancelButton;

        private readonly Action<Window> _removeWindow;
        private Window _filedialogWindow;


        public FileDialogContent(Action<Window> removeWindow)
        {
            _removeWindow = removeWindow;
            _filedialogWindow = new Window();

            _cancelButton = new ButtonWidget
            {
                X = 0,
                Y = 0,
                Width = 80,
                Height = 30,
                Text = "Cancel",
                OnClick = () =>
                {

                    if (_filedialogWindow != null)
                        _removeWindow(_filedialogWindow);
                }
            };
        }


        public void DrawContent(Canvas canvas)
        {
            _openButton = new ButtonWidget
            {
                X = canvas.Width - 180,
                Y = canvas.Height - 40,
                Width = 80,
                Height = 30,
                Text = "Open",
                OnClick = () => { /* Handle open action */ }
            };

            // Logic to get the files and directories in the current path
            try
            {
                // Get all files in the current directory
                foreach (var file in Directory.GetFiles(_currentPath))
                {
                    _filesAndDirectoriesInDirectory.Add(Path.GetFileName(file));
                }

                // Get all directories in the current directory
                foreach (var dir in Directory.GetDirectories(_currentPath))
                {
                    _filesAndDirectoriesInDirectory.Add(Path.GetFileName(dir) + "/");
                }
            }
            catch (Exception ex)
            {
                // For now let's just panic if we can't access the directory.
                // In a real application, we would want to handle this more gracefully.
                PanicHandler.Panic($"Error accessing directory '{_currentPath}': {ex.Message}");
            }

            _cancelButton.Draw(canvas);
            _openButton.Draw(canvas);
        }

        public void HandleInput(int mouseX, int mouseY, bool leftButtonDown)
        {
            _openButton.HandleInput(mouseX, mouseY, leftButtonDown);
            _cancelButton.HandleInput(mouseX, mouseY, leftButtonDown);
        }

        public string WindowTitle(string title)
        {
            return title ?? "File Dialog";
        }
    }
}

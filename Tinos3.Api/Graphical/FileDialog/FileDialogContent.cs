using Cosmos.Kernel.System.Graphics;
using Cosmos.Kernel.System.Input;
using System;
using System.Collections.Generic;
using System.Text;
using Tinos3.Api.Graphical.Widgets;
using Tinos3.Api.Graphical.Windows;
using System.IO;
using Tinos3.Api.Utilities;
using Tinos3.Api.Graphical.Utils.MsgBox;
using System.Drawing;

namespace Tinos3.Api.Graphical.FileDialog
{
    public class FileDialogContent : IWindowContent, IWindowInputHandler
    {
        private string _currentPath = "/";
        private string _selectedFile = string.Empty;
        private List<string> _filesAndDirectoriesInDirectory = new List<string>();
        private bool _directoryLoaded;
        private bool _wasLeftButtonDown;
        private int _lastClickedItem = -1;
        private int _contentWidth;

        private MsgBoxContent _msgBoxContent = new MsgBoxContent();

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
        private readonly Action<string> _onFileSelected;
        private Window _filedialogWindow;

        public FileDialogContent(Action<Window> removeWindow, Action<string> onFileSelected = null)
        {
            _removeWindow = removeWindow;
            _onFileSelected = onFileSelected;

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
            _contentWidth = canvas.Width;
            if (_openButton == null)
            {
                _openButton = new ButtonWidget
                {
                    X = canvas.Width - 180,
                    Y = canvas.Height - 40,
                    Width = 80,
                    Height = 30,
                    Text = "Open",
                    OnClick = () =>
                    {
                        if (!string.IsNullOrEmpty(_selectedFile))
                        {
                            if (_onFileSelected != null)
                            {
                                _onFileSelected(_selectedFile);
                                if (_filedialogWindow != null)
                                    _removeWindow(_filedialogWindow);
                            }
                            else
                            {
                                _msgBoxContent.Message = $"Selected: {_selectedFile}";
                                _msgBoxContent.Title = "File Selected";
                                _msgBoxContent.Type = MsgBoxType.Information;
                                _msgBoxContent.CreateMsgBox();
                            }
                        }
                    }
                };
            }

            if (!_directoryLoaded)
                LoadDirectory();

            RenderFilesAndDirectories(canvas);

            _cancelButton.Draw(canvas);
            _openButton.Draw(canvas);
        }

        public void HandleInput(int mouseX, int mouseY, bool leftButtonDown)
        {
            _openButton.HandleInput(mouseX, mouseY, leftButtonDown);
            _cancelButton.HandleInput(mouseX, mouseY, leftButtonDown);

            if (leftButtonDown && !_wasLeftButtonDown)
            {
                int itemIndex = (mouseY - 40) / 25;
                if (mouseY >= 40 && itemIndex >= 0 && itemIndex < _filesAndDirectoriesInDirectory.Count
                    && mouseX >= 10 && mouseX < _contentWidth - 10)
                {
                    string item = _filesAndDirectoriesInDirectory[itemIndex];
                    if (item.EndsWith("/") && _lastClickedItem == itemIndex)
                    {
                        _currentPath = Path.Combine(_currentPath, item.Substring(0, item.Length - 1));
                        _selectedFile = string.Empty;
                        _directoryLoaded = false;
                        _lastClickedItem = -1;
                    }
                    else
                    {
                        _selectedFile = Path.Combine(_currentPath, item.TrimEnd('/'));
                        _lastClickedItem = itemIndex;
                    }
                }
            }

            _wasLeftButtonDown = leftButtonDown;
        }

        public string WindowTitle(string title)
        {
            return title ?? "File Dialog";
        }

        public string FileDialogResult()
        {
            if (string.IsNullOrEmpty(_selectedFile))
            {
                _msgBoxContent.Message = "No file selected.";
                _msgBoxContent.Title = "Warning";
                _msgBoxContent.Type = MsgBoxType.Warning;
                _msgBoxContent.CreateMsgBox();
                return null;
            }

            return _selectedFile;
        }

        public void SetWindow(Window window)
        {
            _filedialogWindow = window;
        }

        private void RenderFilesAndDirectories(Canvas canvas)
        {
            int yOffset = 40;
            for (int i = 0; i < _filesAndDirectoriesInDirectory.Count; i++)
            {
                string item = _filesAndDirectoriesInDirectory[i];
                bool isSelected = Path.Combine(_currentPath, item.TrimEnd('/')) == _selectedFile;
                Color background = isSelected ? Color.LightBlue : Color.White;
                Color foreground = isSelected ? Color.DarkBlue : Color.Black;
                canvas.DrawFilledRectangle(background, 10, yOffset, canvas.Width - 20, 24);
                canvas.DrawRectangle(Color.Gray, 10, yOffset, canvas.Width - 20, 24);
                new Label { X = 16, Y = yOffset + 3, Text = item }.Draw(canvas, foreground);
                yOffset += 25;
            }
        }

        private void LoadDirectory()
        {
            _filesAndDirectoriesInDirectory.Clear();
            try
            {
                foreach (var dir in Directory.GetDirectories(_currentPath))
                    _filesAndDirectoriesInDirectory.Add(Path.GetFileName(dir) + "/");
                foreach (var file in Directory.GetFiles(_currentPath))
                    _filesAndDirectoriesInDirectory.Add(Path.GetFileName(file));
                _directoryLoaded = true;
            }
            catch (Exception ex)
            {
                _msgBoxContent.Message = $"Error accessing directory '{_currentPath}': {ex.Message}";
                _msgBoxContent.Title = "Error";
                _msgBoxContent.Type = MsgBoxType.Error;
                _msgBoxContent.CreateMsgBox();
                _directoryLoaded = true;
            }
        }
    }
}

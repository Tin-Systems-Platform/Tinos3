using Cosmos.Executable.Lua;
using Cosmos.Kernel.System.Graphics;
using System;
using System.Drawing;
using System.IO;
using System.Text;
using Tinos3.Api.Graphical.Widgets;
using Tinos3.Api.Graphical.Utils.MsgBox;
using Tinos3.Api.Graphical.Windows;
using Tinos3.Graphical.Apps.System.Notepad;

namespace Tinos3.Graphical.Apps.System.FileExplorer
{
    public class FileExplorerContent : IWindowContent, IWindowInputHandler, IWindowKeyInputHandler
    {
        private const int ItemTop = 112;
        private const int ItemHeight = 25;
        private const int MaximumItems = 12;

        private readonly Action<Window> _addWindow;
        private readonly Action<Window> _removeWindow;
        private readonly List<string> _entries = new List<string>();
        private readonly TextBox _pathBox = new TextBox { X = 12, Y = 42, Width = 350, Height = 30 };
        private readonly ButtonWidget _goButton;
        private readonly ButtonWidget _upButton;
        private readonly ButtonWidget _openButton;
        private readonly ButtonWidget _refreshButton;
        private string _currentPath = "/";
        private string _selectedEntry = string.Empty;
        private string _status = "Ready";
        private bool _loaded;
        private bool _wasLeftButtonDown;
        private int _contentWidth;
        private string _pendingError = string.Empty;

        public FileExplorerContent(Action<Window> addWindow, Action<Window> removeWindow)
        {
            _addWindow = addWindow;
            _removeWindow = removeWindow;
            _pathBox.SetText(_currentPath);
            _goButton = new ButtonWidget { X = 368, Y = 42, Width = 48, Height = 30, Text = "Go", OnClick = NavigateToPath };
            _upButton = new ButtonWidget { X = 422, Y = 42, Width = 48, Height = 30, Text = "Up", OnClick = NavigateUp };
            _refreshButton = new ButtonWidget { X = 476, Y = 42, Width = 60, Height = 30, Text = "Refresh", OnClick = () => { _loaded = false; } };
            _openButton = new ButtonWidget { X = 12, Y = 74, Width = 80, Height = 30, Text = "Open", OnClick = OpenSelected };
        }

        public void DrawContent(Canvas canvas)
        {
            _contentWidth = canvas.Width;
            _pathBox.Draw(canvas);
            _goButton.Draw(canvas);
            _upButton.Draw(canvas);
            _refreshButton.Draw(canvas);
            if (!_loaded) LoadDirectory();

            int visibleCount = Math.Min(_entries.Count, Math.Min(MaximumItems, Math.Max(1, (canvas.Height - ItemTop - 40) / ItemHeight)));
            for (int i = 0; i < visibleCount; i++)
            {
                string entry = _entries[i];
                bool selected = entry == _selectedEntry;
                int y = ItemTop + i * ItemHeight;
                canvas.DrawFilledRectangle(selected ? Color.LightBlue : Color.White, 12, y, canvas.Width - 24, ItemHeight - 1);
                canvas.DrawRectangle(Color.Gray, 12, y, canvas.Width - 24, ItemHeight - 1);
                GraphicalFont.DrawString(canvas, entry, selected ? Color.DarkBlue : Color.Black, 18, y + 3);
            }

            _openButton.Draw(canvas);
            GraphicalFont.DrawString(canvas, _status, Color.Black, 12, canvas.Height - 24);
        }

        public void HandleInput(int mouseX, int mouseY, bool leftButtonDown)
        {
            ShowPendingError();
            _pathBox.HandleInput(mouseX, mouseY, leftButtonDown);
            _goButton.HandleInput(mouseX, mouseY, leftButtonDown);
            _upButton.HandleInput(mouseX, mouseY, leftButtonDown);
            _refreshButton.HandleInput(mouseX, mouseY, leftButtonDown);
            _openButton.HandleInput(mouseX, mouseY, leftButtonDown);

            if (leftButtonDown && !_wasLeftButtonDown && mouseY >= ItemTop && mouseX >= 12 && mouseX < _contentWidth - 12)
            {
                int index = (mouseY - ItemTop) / ItemHeight;
                if (index >= 0 && index < _entries.Count && index < MaximumItems)
                    _selectedEntry = _entries[index];
            }
            _wasLeftButtonDown = leftButtonDown;
        }

        public void HandleKeyInput(ConsoleKeyInfo keyInfo)
        {
            if (keyInfo.Key == ConsoleKey.Enter && _pathBox.IsFocused)
            {
                NavigateToPath();
                return;
            }
            _pathBox.HandleInput(keyInfo);
        }

        public string WindowTitle(string title) => title ?? "File Explorer";

        private void NavigateToPath()
        {
            _currentPath = _pathBox.Text.Trim();
            if (_currentPath.Length == 0) _currentPath = "/";
            _selectedEntry = string.Empty;
            _loaded = false;
        }

        private void NavigateUp()
        {
            string parent = Path.GetDirectoryName(_currentPath.TrimEnd('/', '\\'));
            _currentPath = string.IsNullOrEmpty(parent) ? "/" : parent;
            _pathBox.SetText(_currentPath);
            _selectedEntry = string.Empty;
            _loaded = false;
        }

        private void LoadDirectory()
        {
            _entries.Clear();
            try
            {
                foreach (string directory in Directory.GetDirectories(_currentPath))
                    _entries.Add(Path.GetFileName(directory) + "/");
                foreach (string file in Directory.GetFiles(_currentPath))
                    _entries.Add(Path.GetFileName(file));
                _status = _entries.Count == 0 ? "This directory is empty" : _entries.Count + " item(s)";
            }
            catch (Exception ex)
            {
                ReportError("Cannot open directory", ex.Message);
            }
            _pathBox.SetText(_currentPath);
            _loaded = true;
        }

        private void OpenSelected()
        {
            if (string.IsNullOrEmpty(_selectedEntry)) { ReportError("Nothing selected", "Select a file or directory to open."); return; }
            string path = Path.Combine(_currentPath, _selectedEntry.TrimEnd('/', '\\'));
            if (_selectedEntry.EndsWith("/"))
            {
                _currentPath = path;
                _pathBox.SetText(_currentPath);
                _selectedEntry = string.Empty;
                _loaded = false;
                return;
            }

            if (string.Equals(Path.GetExtension(path), ".lua", StringComparison.OrdinalIgnoreCase))
            {
                RunLuaFile(path);
                return;
            }

            try
            {
                byte[] bytes = File.ReadAllBytes(path);
                string text = DecodeText(bytes);
                NotepadContent notepad = new NotepadContent { FilePath = path, TextContent = text };
                notepad.MarkAsSaved();
                _addWindow?.Invoke(new Window(100, 100, 540, 400, "Notepad", notepad));
                _status = "Opened " + _selectedEntry;
            }
            catch (InvalidDataException)
            {
                ReportError("Cannot open file", "Binary files cannot be opened in Notepad.");
            }
            catch (Exception ex)
            {
                ReportError("Open failed", ex.Message);
            }
        }

        private static string DecodeText(byte[] bytes)
        {
            try
            {
                if (bytes.Length >= 2 && bytes[0] == 0xFF && bytes[1] == 0xFE)
                    return new UnicodeEncoding(false, true, true).GetString(bytes, 2, bytes.Length - 2);
                if (bytes.Length >= 2 && bytes[0] == 0xFE && bytes[1] == 0xFF)
                    return new UnicodeEncoding(true, true, true).GetString(bytes, 2, bytes.Length - 2);
                if (bytes.Length >= 3 && bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF)
                    return new UTF8Encoding(false, true).GetString(bytes, 3, bytes.Length - 3);
            }
            catch (DecoderFallbackException)
            {
                throw new InvalidDataException("Binary content");
            }

            for (int i = 0; i < bytes.Length; i++)
                if (bytes[i] == 0) throw new InvalidDataException("Binary content");
            try { return new UTF8Encoding(false, true).GetString(bytes); }
            catch (DecoderFallbackException) { throw new InvalidDataException("Binary content"); }
        }

        private void RunLuaFile(string path)
        {
            try
            {
                LuaInterpreter lua = new LuaInterpreter();
                lua.DoString(File.ReadAllText(path));
                _status = "Ran " + _selectedEntry;
            }
            catch (Exception ex)
            {
                ReportError("Lua execution failed\n", ex.Message);
            }
        }

        private void ReportError(string title, string message)
        {
            _status = title;
            _pendingError = title + ": " + message;
        }

        private void ShowPendingError()
        {
            if (_pendingError.Length == 0 || _addWindow == null) return;

            string message = _pendingError;
            _pendingError = string.Empty;
            MsgBoxContent content = new MsgBoxContent("File Explorer Error", message, MsgBoxType.Error, _removeWindow);
            Window window = new Window(180, 160, 360, 180, "File Explorer Error", content);
            content.SetWindow(window);
            _addWindow(window);
        }
    }
}

using Cosmos.Kernel.System.Graphics;
using System;
using System.Drawing;
using System.IO;
using Tinos3.Api.Graphical.FileDialog;
using Tinos3.Api.Graphical.Widgets;
using Tinos3.Api.Graphical.Windows;

namespace Tinos3.Graphical.Apps.System.Notepad
{
    public class NotepadContent : IWindowContent, IWindowInputHandler, IWindowKeyInputHandler
    {
        private const int EditorX = 12;
        private const int EditorY = 88;
        private const int LineHeight = 24;

        private string _textContent = string.Empty;
        private string _filePath = "/mnt/untitled.txt";
        private string _status = "Ready";
        private int _cursor;
        private int _firstVisibleLine;
        private int _visibleLines = 10;
        private bool _isDirty;
        private bool _editorFocused;
        private readonly Action<Window> _addWindow;
        private readonly Action<Window> _removeWindow;

        private readonly TextBox _pathBox = new TextBox
        {
            X = 12,
            Y = 44,
            Width = 300,
            Height = 30
        };

        private readonly ButtonWidget _newButton;
        private readonly ButtonWidget _openButton;
        private readonly ButtonWidget _saveButton;

        public NotepadContent() : this(null, null)
        {
        }

        public NotepadContent(Action<Window> addWindow, Action<Window> removeWindow)
        {
            _addWindow = addWindow;
            _removeWindow = removeWindow;
            _pathBox.SetText(_filePath);
            _newButton = new ButtonWidget { X = 322, Y = 44, Width = 60, Height = 30, Text = "New", OnClick = NewFile };
            _openButton = new ButtonWidget { X = 388, Y = 44, Width = 60, Height = 30, Text = "Open", OnClick = OpenDialog };
            _saveButton = new ButtonWidget { X = 454, Y = 44, Width = 60, Height = 30, Text = "Save", OnClick = SaveFile };
        }

        public string TextContent
        {
            get => _textContent;
            set { _textContent = NormalizeNewlines(value ?? string.Empty); _cursor = _textContent.Length; _isDirty = true; }
        }

        public string FilePath
        {
            get => _filePath;
            set { _filePath = value ?? string.Empty; _pathBox.SetText(_filePath); }
        }

        public string FileName => string.IsNullOrEmpty(_filePath) ? "Untitled" : Path.GetFileName(_filePath);

        public void MarkAsSaved()
        {
            _isDirty = false;
            _status = "Opened " + FileName;
        }

        public void DrawContent(Canvas canvas)
        {
            _visibleLines = Math.Max(1, (canvas.Height - EditorY - 28) / LineHeight);
            _pathBox.Draw(canvas);
            _newButton.Draw(canvas);
            _openButton.Draw(canvas);
            _saveButton.Draw(canvas);

            canvas.DrawFilledRectangle(Color.White, EditorX, EditorY, canvas.Width - (EditorX * 2), canvas.Height - EditorY - 24);
            canvas.DrawRectangle(_editorFocused ? Color.Blue : Color.Gray, EditorX, EditorY, canvas.Width - (EditorX * 2), canvas.Height - EditorY - 24);
            DrawEditorText(canvas);

            GraphicalFont.DrawString(canvas, (_isDirty ? "Modified  |  " : string.Empty) + _status, Color.Black, EditorX, canvas.Height - 22);
        }

        public void HandleInput(int mouseX, int mouseY, bool leftButtonDown)
        {
            _pathBox.HandleInput(mouseX, mouseY, leftButtonDown);
            _newButton.HandleInput(mouseX, mouseY, leftButtonDown);
            _openButton.HandleInput(mouseX, mouseY, leftButtonDown);
            _saveButton.HandleInput(mouseX, mouseY, leftButtonDown);

            if (leftButtonDown)
                _editorFocused = mouseX >= EditorX && mouseY >= EditorY;
        }

        public void HandleKeyInput(ConsoleKeyInfo keyInfo)
        {
            if (keyInfo.Modifiers.HasFlag(ConsoleModifiers.Control))
            {
                if (keyInfo.Key == ConsoleKey.S) { SaveFile(); return; }
                if (keyInfo.Key == ConsoleKey.O) { OpenDialog(); return; }
                if (keyInfo.Key == ConsoleKey.N) { NewFile(); return; }
            }

            if (_pathBox.IsFocused)
            {
                _pathBox.HandleInput(keyInfo);
                _filePath = _pathBox.Text;
                return;
            }

            if (!_editorFocused)
                return;

            switch (keyInfo.Key)
            {
                case ConsoleKey.Backspace:
                    if (_cursor > 0) { _textContent = _textContent.Remove(_cursor - 1, 1); _cursor--; MarkModified(); }
                    break;
                case ConsoleKey.Delete:
                    if (_cursor < _textContent.Length) { _textContent = _textContent.Remove(_cursor, 1); MarkModified(); }
                    break;
                case ConsoleKey.Enter:
                    Insert("\n");
                    break;
                case ConsoleKey.LeftArrow:
                    if (_cursor > 0) _cursor--;
                    break;
                case ConsoleKey.RightArrow:
                    if (_cursor < _textContent.Length) _cursor++;
                    break;
                case ConsoleKey.Home:
                    int previousLineBreak = _cursor == 0 ? -1 : _textContent.LastIndexOf('\n', _cursor - 1);
                    _cursor = previousLineBreak + 1;
                    break;
                case ConsoleKey.End:
                    int nextLine = _textContent.IndexOf('\n', _cursor);
                    _cursor = nextLine < 0 ? _textContent.Length : nextLine;
                    break;
                case ConsoleKey.UpArrow:
                    MoveVertical(-1);
                    break;
                case ConsoleKey.DownArrow:
                    MoveVertical(1);
                    break;
                default:
                    if (!char.IsControl(keyInfo.KeyChar)) Insert(keyInfo.KeyChar.ToString());
                    break;
            }
        }

        public string WindowTitle(string title) => "Notepad - " + FileName + (_isDirty ? " *" : string.Empty);

        private void NewFile()
        {
            _textContent = string.Empty;
            _cursor = 0;
            _isDirty = false;
            _filePath = "/mnt/untitled.txt";
            _pathBox.SetText(_filePath);
            _status = "New document";
            _editorFocused = true;
        }

        private void OpenFile(string path)
        {
            if (path.Length == 0) { _status = "Enter a file path"; return; }
            try
            {
                _textContent = NormalizeNewlines(File.ReadAllText(path));
                _filePath = path;
                _cursor = 0;
                _isDirty = false;
                _status = "Opened " + FileName;
                _editorFocused = true;
            }
            catch (Exception ex) { _status = "Open failed: " + ex.Message; }
        }

        private void OpenDialog()
        {
            if (_addWindow == null || _removeWindow == null)
            {
                _status = "File dialog is unavailable";
                return;
            }

            FileDialogContent dialogContent = new FileDialogContent(_removeWindow, OpenFile);
            Window dialogWindow = new Window(100, 100, 540, 400, "Open File", dialogContent);
            dialogContent.SetWindow(dialogWindow);
            _addWindow(dialogWindow);
        }

        private void SaveFile()
        {
            string path = _pathBox.Text.Trim();
            if (path.Length == 0) { _status = "Enter a file path"; return; }
            try
            {
                File.WriteAllText(path, _textContent);
                _filePath = path;
                _isDirty = false;
                _status = "Saved " + FileName;
            }
            catch (Exception ex) { _status = "Save failed: " + ex.Message; }
        }

        private void Insert(string value)
        {
            _textContent = _textContent.Insert(_cursor, value);
            _cursor += value.Length;
            MarkModified();
        }

        private void MarkModified()
        {
            _isDirty = true;
            _status = "Ready";
        }

        private void MoveVertical(int direction)
        {
            int previousLineBreak = _cursor == 0 ? -1 : _textContent.LastIndexOf('\n', _cursor - 1);
            int lineStart = previousLineBreak + 1;
            int column = _cursor - lineStart;
            if (direction < 0)
            {
                if (lineStart == 0) return;
                int targetEnd = lineStart - 1;
                int targetPreviousBreak = targetEnd == 0 ? -1 : _textContent.LastIndexOf('\n', targetEnd - 1);
                int targetStart = targetPreviousBreak + 1;
                _cursor = targetStart + Math.Min(column, targetEnd - targetStart);
                return;
            }

            int currentEnd = _textContent.IndexOf('\n', _cursor);
            if (currentEnd < 0) return;
            int nextStart = currentEnd + 1;
            int nextEnd = _textContent.IndexOf('\n', nextStart);
            if (nextEnd < 0) nextEnd = _textContent.Length;
            _cursor = nextStart + Math.Min(column, nextEnd - nextStart);
        }

        private void DrawEditorText(Canvas canvas)
        {
            string[] lines = _textContent.Split('\n');
            int cursorLine = 0;
            int cursorColumn = 0;
            int count = 0;
            for (int i = 0; i < _cursor; i++)
            {
                if (_textContent[i] == '\n') { cursorLine++; cursorColumn = 0; }
                else cursorColumn++;
            }

            if (cursorLine < _firstVisibleLine) _firstVisibleLine = cursorLine;
            if (cursorLine >= _firstVisibleLine + _visibleLines) _firstVisibleLine = cursorLine - _visibleLines + 1;
            for (int line = _firstVisibleLine; line < lines.Length && count < _visibleLines; line++, count++)
                GraphicalFont.DrawString(canvas, lines[line], Color.Black, EditorX + 5, EditorY + 3 + count * LineHeight);

            if (_editorFocused && cursorLine >= _firstVisibleLine && cursorLine < _firstVisibleLine + _visibleLines)
            {
                string currentLine = lines[cursorLine];
                int x = EditorX + 5 + GraphicalFont.MeasureString(currentLine.Substring(0, cursorColumn));
                int y = EditorY + 3 + (cursorLine - _firstVisibleLine) * LineHeight;
                canvas.DrawLine(Color.Black, x, y, x, y + GraphicalFont.DefaultSize);
            }
        }

        private static string NormalizeNewlines(string text) => text.Replace("\r\n", "\n").Replace('\r', '\n');
    }
}

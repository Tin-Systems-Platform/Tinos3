using System;
using System.Collections.Generic;
using System.Text;

namespace Tinos3.Graphical.Apps.System.Notepad
{
    public class NotepadContent
    {
        private string _textContent = string.Empty;

        private string _filePath = string.Empty;

        private string _fileName = "Untitled";

        public string TextContent
        {
            get => _textContent;
            set => _textContent = value;
        }

        public string FilePath
        {
            get => _filePath;
            set => _filePath = value;
        }

        public string FileName
        {
            get => _fileName;
            set => _fileName = value;
        }
    }
}

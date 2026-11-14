using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Tinos3
{
    public static partial class Native
    {
        [LibraryImport("test")]
        public static partial void test();

        [LibraryImport("get_status_text", EntryPoint = "get_status_text")]
        public static partial int GetStatusText(Span<byte> buffer);
    }
}

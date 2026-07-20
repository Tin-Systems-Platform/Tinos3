using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Tinos3.Abstractions.FileTypes.TXE
{
    public interface ITXEHeader
    {
        string Magic { get; }          // "TXE"
        ushort Version { get; }

        string Name { get; }

        Architecture Architecture { get; }

        ExecutableFlags Flags { get; }

        ulong EntryPoint { get; }

        uint HeaderSize { get; }

        Boolean TinosOnly { get; }




    }
}

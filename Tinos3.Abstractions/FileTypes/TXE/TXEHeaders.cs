using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Tinos3.Abstractions.FileTypes.TXE
{
    public class TXEHeaders : ITXEHeader
    {
        private readonly string magic;
        private readonly ushort ver;
        private readonly Architecture arch;
        private readonly string name;
        private readonly ExecutableFlags flags;
        private readonly ulong entry;
        private readonly uint headerSize;
        private readonly bool isTinosOnly;


        public TXEHeaders(
            string magic,
            ushort version,
            string name,
            Architecture architecture,
            ExecutableFlags flags,
            ulong entryPoint,
            uint headerSize,
            bool tinosOnly)
        {
            this.magic = magic;
            this.ver = version;
            this.name = name;
            this.arch = architecture;
            this.flags = flags;
            this.entry = entryPoint;
            this.headerSize = headerSize;
            this.isTinosOnly = tinosOnly;

            if (magic != "TXE")
                throw new ArgumentException("Invalid TXE magic.");
        }

        public string Magic => magic;

        public ushort Version => ver;

        public string Name => name;

        public Architecture Architecture => arch;

        public ExecutableFlags Flags => flags;

        public ulong EntryPoint => entry;

        public uint HeaderSize => headerSize;

        public bool TinosOnly => isTinosOnly;
    }
}

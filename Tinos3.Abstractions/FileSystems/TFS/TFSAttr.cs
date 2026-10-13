using System;
using System.Collections.Generic;
using System.Text;

namespace Tinos3.Abstractions.FileSystems.TFS
{
    [Flags]
    public enum TFSAttr : byte
    {
        None = 0x00,
        ReadOnly = 0x01,
        Hidden = 0x02,
        System = 0x04,
        VolumeID = 0x08,
        Directory = 0x10,
        Archive = 0x20,

        Lfn = ReadOnly | Hidden | System | VolumeID,
    }
}

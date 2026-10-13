
/*
 * Portions of this file are derived from Cosmos.
 *
 * Copyright (c) 2007-2026, CosmosOS, COSMOS Project
 * Licensed under the BSD 3-Clause License.
 *
 * TFS-specific modifications and additional code:
 * Copyright (c) 2026 Tinos3 contributors.
 *
 * The full BSD 3-Clause license terms and disclaimer
 * are available in THIRD-PARTY-LICENSES/Cosmos-BSD-3-Clause.txt.
 */


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

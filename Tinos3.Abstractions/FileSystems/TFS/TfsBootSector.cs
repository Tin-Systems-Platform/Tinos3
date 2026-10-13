
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


using Cosmos.Kernel.System.FileSystem.Fat;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tinos3.Abstractions.FileSystems.TFS
{
    public sealed class TfsBootSector
    {
        public const ulong BootSectorLba = 0;
        internal const ushort BootSignature = 0xAA55;
        internal const int BootSignatureOffset = 510;
        internal const int JmpBootOffset = 0;
        internal const int JmpBootDisplacementOffset = JmpBootOffset + 1;
        internal const int JmpBootNopOffset = JmpBootOffset + 2;
        internal const int OemNameOffset = 3;
        internal const int OemNameLength = 8;
        internal const int BytsPerSecOffset = 11;
        internal const int SecPerClusOffset = 13;
        internal const int RsvdSecCntOffset = 14;
        internal const int NumFatsOffset = 16;
        internal const int RootEntCntOffset = 17;
        internal const int TotSec16Offset = 19;
        internal const int MediaOffset = 21;
        internal const int FatSz16Offset = 22;
        internal const int SecPerTrkOffset = 24;
        internal const int NumHeadsOffset = 26;
        internal const int HiddSecOffset = 28;
        internal const int TotSec32Offset = 32;
        internal const int Fat1216DrvNumOffset = 36;
        internal const int FatSz32Offset = 36;
        internal const int ExtFlagsOffset = 40;
        internal const int FsVerOffset = 42;
        internal const int RootClusOffset = 44;
        internal const int FsInfoOffset = 48;
        internal const int BkBootSecOffset = 50;
        internal const int Tfs1DrvNumOffset = 64;
        internal const int Tfs1BootSigOffset = 66;
        internal const int Tfs1VolIdOffset = 67;
        internal const int Tfs1VolLabOffset = 71;
        internal const int Tfs1FilSysTypeOffset = 82;
        internal const int UInt16FieldSize = 2;
        internal const int UInt32FieldSize = 4;
        internal const int VolumeLabelLength = 11;
        internal const int FilSysTypeLength = 8;
        internal const uint MinBytesPerSector = 512;
        internal const uint MaxBytesPerSector = 4096;
        private const uint MaxSectorsPerCluster = 128;
        private const uint Tfs1MinClusters = 65525;
        public TfsType Type { get; private set; }
        public uint BytesPerSector { get; private set; }
        public uint SectorsPerCluster { get; private set; }
        public uint BytesPerCluster { get; private set; }
        public uint ReservedSectorCount { get; private set; }
        public uint RootEntryCount { get; private set; }
        public uint TotalSectorCount { get; private set; }
        public uint FatSectorCount { get; private set; }
        public uint RootCluster { get; private set; }
        public uint RootStartLba { get; private set; }
        public uint RootSectorCount { get; private set; }
        public uint DataStartLba { get; private set; }
        public uint ClusterCount { get; private set; }
        public uint FatStartLba { get; private set; }
    }
}

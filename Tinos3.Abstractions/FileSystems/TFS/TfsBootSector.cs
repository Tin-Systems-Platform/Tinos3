
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
using System.Data;
using System.Diagnostics.CodeAnalysis;
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

        // Blank and private constructor for TryParse method
        private TfsBootSector() { }

        public static bool TryParse(ReadOnlySpan<byte> bpb, [NotNullWhen(true)] out TfsBootSector? bootSector)
        {
            bootSector = null;

            if (bpb.Length < (int)MinBytesPerSector)
            {
                return false;
            }

            if (BitConverter.ToUInt16(bpb.Slice(BootSignatureOffset, UInt16FieldSize)) != BootSignature)
            {
                return false;
            }

            TfsBootSector bs = new()
            {
                BytesPerSector = BitConverter.ToUInt16(bpb.Slice(BytsPerSecOffset, UInt16FieldSize)),
                SectorsPerCluster = bpb[SecPerClusOffset],
                ReservedSectorCount = BitConverter.ToUInt16(bpb.Slice(RsvdSecCntOffset, UInt16FieldSize)),
                RootEntryCount = BitConverter.ToUInt16(bpb.Slice(RootEntCntOffset, UInt16FieldSize)),
            };

            if (bs.BytesPerSector < MinBytesPerSector || bs.BytesPerSector > MaxBytesPerSector
                || (bs.BytesPerSector & (bs.BytesPerSector - 1)) != 0
                || bs.SectorsPerCluster == 0 || bs.SectorsPerCluster > MaxSectorsPerCluster
                || (bs.SectorsPerCluster & (bs.SectorsPerCluster - 1)) != 0)
            {
                return false;
            }

            bs.BytesPerCluster = bs.BytesPerSector * bs.SectorsPerCluster;

            uint total16 = BitConverter.ToUInt16(bpb.Slice(TotSec16Offset, UInt16FieldSize));
            bs.TotalSectorCount = total16 != 0 ? total16 : BitConverter.ToUInt32(bpb.Slice(TotSec32Offset, UInt32FieldSize));

            uint fat16 = BitConverter.ToUInt16(bpb.Slice(FatSz16Offset, UInt16FieldSize));
            bs.FatSectorCount = fat16 != 0 ? fat16 : BitConverter.ToUInt32(bpb.Slice(FatSz32Offset, UInt32FieldSize));

            if (bs.TotalSectorCount == 0 || bs.FatSectorCount == 0)
            {
                return false;
            }

            bs.FatStartLba = bs.ReservedSectorCount;
            bs.RootSectorCount = (bs.RootEntryCount * (uint)TfsDirectory.EntrySize + (bs.BytesPerSector - 1)) / bs.BytesPerSector;

            ulong fatRegion = (ulong)bs.NumberOfFats * bs.FatSectorCount;
            ulong rootStart = bs.ReservedSectorCount + fatRegion;
            ulong dataStart = rootStart + bs.RootSectorCount;
            if (dataStart >= bs.TotalSectorCount)
            {
                return false;
            }
            bs.RootStartLba = (uint)rootStart;
            bs.DataStartLba = (uint)dataStart;

            uint dataSectorCount = bs.TotalSectorCount - bs.DataStartLba;
            bs.ClusterCount = dataSectorCount / bs.SectorsPerCluster;
            if (bs.ClusterCount == 0)
            {
                return false;
            }

            if (bs.Type == FatType.Fat32)
            {
                bs.RootCluster = BitConverter.ToUInt32(bpb.Slice(RootClusOffset, UInt32FieldSize));
                bs.RootStartLba = 0;
                bs.RootSectorCount = 0;
                bs.DataStartLba = (uint)rootStart;
            }
            else
            {
                bs.RootCluster = 0;
            }

            bootSector = bs;
            return true;
        }

        public ulong ClusterToLba(uint cluster)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(cluster, TfsTable.FirstDataCluster);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(cluster, ClusterCount + 1);
            return DataStartLba + ((ulong)(cluster - TfsTable.FirstDataCluster)) * SectorsPerCluster;
        }
    }
}

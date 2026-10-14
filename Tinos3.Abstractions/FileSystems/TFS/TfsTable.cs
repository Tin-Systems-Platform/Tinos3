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

using Cosmos.Kernel.HAL.Devices.Storage;
using Cosmos.Kernel.System.FileSystem.Fat;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tinos3.Abstractions.FileSystems.TFS
{
    internal sealed class TfsTable
    {
        public const uint FreeCluster = 0x0000_0000;
        public const uint FirstDataCluster = 2;
        public const uint Fat32EndOfChain = 0x0FFF_FFF8;
        public const uint Fat32BadCluster = 0x0FFF_FFF7;
        internal const uint Fat32EndOfChainValue = 0x0FFF_FFFF;
        private const uint Fat32EntryMask = 0x0FFF_FFFF;
        private const uint Fat32ReservedMask = 0xF000_0000;
        private const int BitsPerByte = 8;
        private const int LowByteMask = 0xFF;
        private readonly IBlockDevice _device;
        private readonly TfsBootSector _boot;
        private readonly uint _clusterLimit;
        private readonly byte[] _fatSector;
        private readonly byte[] _fatSpill;
        internal const uint Fat32EntrySize = 4;
        private ulong _fatSectorLba = ulong.MaxValue;

        private uint _nextFreeHint = FirstDataCluster;

        public TfsTable(IBlockDevice device, TfsBootSector boot)
        {
            _device = device;
            _boot = boot;
            _fatSector = new byte[boot.BytesPerSector];
            _fatSpill = new byte[boot.BytesPerSector];

            // Entries one FAT copy can hold (FAT12 packs 2 entries per 3 bytes).
            ulong fatBytes = (ulong)boot.FatSectorCount * boot.BytesPerSector;
            ulong fatCapacity = boot.Type switch
            {
                TfsType.Tfs1 => fatBytes / Fat32EntrySize,
            };
            ulong limit = (ulong)boot.ClusterCount + FirstDataCluster;
            if (fatCapacity < limit)
            {
                limit = fatCapacity;
            }
            _clusterLimit = (uint)limit;
        }


    }
}

using Cosmos.Kernel.HAL.Devices.Storage;
using Cosmos.Kernel.System.Storage;
using Tinos3.FS.Helpers;
using ApiPartitionManager = Tinos.Api.Common.FS.Partitions.TinPartManager;
using ApiPartitionTableKind = Tinos.Api.Common.FS.Partitions.PartitionTableKind;

namespace Tinos3.FS;

/// <summary>
/// Keeps Tinos3's partition manager interface while delegating the shared
/// partition operations to Tinos.Api.Common.
/// </summary>
public class TinosPartManager
{
    private readonly ApiPartitionManager _partitionManager = new();

    public void ListPartitions(int diskIndex)
    {
        _partitionManager.ListPartitions(diskIndex);
    }

    // Keep initialization explicit and refuse to replace an existing table.
    public bool InitializePartitionTable(int diskIndex, PartitionTableKind kind)
    {
        IBlockDevice? disk = GetDisk(diskIndex);

        if (disk is null)
            return false;

        if (Cosmos.Kernel.System.Storage.Gpt.IsGpt(disk) ||
            Cosmos.Kernel.System.Storage.Mbr.IsMbr(disk))
        {
            Console.WriteLine("FS: A partition table already exists");
            return false;
        }

        ApiPartitionTableKind? apiKind = kind switch
        {
            PartitionTableKind.GPT => ApiPartitionTableKind.GPT,
            PartitionTableKind.MBR => ApiPartitionTableKind.MBR,
            _ => null
        };

        if (apiKind is null)
        {
            Console.WriteLine("FS: Unsupported partition table type");
            return false;
        }

        return ApiPartitionManager.InitializePartitionTable(diskIndex, apiKind.Value);
    }

    public bool CreatePartition(
        int diskIndex,
        ulong startSector,
        ulong sectorCount,
        byte mbrSystemId = 0x0C)
    {
        IBlockDevice? disk = GetDisk(diskIndex);

        if (disk is null)
            return false;

        if (!Cosmos.Kernel.System.Storage.Gpt.IsGpt(disk) &&
            !Cosmos.Kernel.System.Storage.Mbr.IsMbr(disk))
        {
            Console.WriteLine("FS: Initialize a partition table first");
            return false;
        }

        return ApiPartitionManager.CreatePartition(
            diskIndex, startSector, sectorCount, mbrSystemId);
    }

    public bool DeletePartition(int diskIndex, ulong startSector, ulong sectorCount)
    {
        return _partitionManager.DeletePartition(diskIndex, startSector, sectorCount);
    }

    public bool ResizePartition(
        int diskIndex,
        ulong startSector,
        ulong oldSectorCount,
        ulong newSectorCount)
    {
        return _partitionManager.ResizePartition(
            diskIndex, startSector, oldSectorCount, newSectorCount);
    }

    private static IBlockDevice? GetDisk(int diskIndex)
    {
        if (diskIndex < 0 || diskIndex >= StorageManager.DeviceCount)
        {
            Console.WriteLine("FS: Invalid disk index");
            return null;
        }

        IBlockDevice? disk = StorageManager.GetDevice(diskIndex);

        if (disk is null)
            Console.WriteLine("FS: No disk detected");

        return disk;
    }
}

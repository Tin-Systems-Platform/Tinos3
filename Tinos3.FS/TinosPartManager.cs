using Tinos3.FS.Helpers;
using Cosmos.Kernel.HAL.Devices.Storage;
using Cosmos.Kernel.System.Storage;

namespace Tinos3.FS;

public class TinosPartManager
{
    public void ListPartitions(int diskIndex)
    {
        IBlockDevice? disk = GetDisk(diskIndex);

        if (disk is null)
        {
            Console.WriteLine("FS: No disk detected");
            return;
        }

        if (Gpt.IsGpt(disk))
        {
            var partitions = Gpt.Parse(disk);
            Console.WriteLine(
                $"FS: GPT, {partitions.Count} partition(s)");
        }
        else if (Mbr.IsMbr(disk))
        {
            var partitions = Mbr.Parse(disk);
            Console.WriteLine(
                $"FS: MBR, {partitions.Count} partition(s)");
        }
        else
        {
            Console.WriteLine(
                "FS: No recognized partition table");
        }
    }

    // Explicitly initialize a partition table.
    // Do not call this automatically during normal startup.
    public bool InitializePartitionTable(int diskIndex, PartitionTableKind kind)
    {
        IBlockDevice? disk = GetDisk(diskIndex);

        if (disk is null)
        {
            Console.WriteLine("FS: No disk detected");
            return false;
        }

        if (Gpt.IsGpt(disk) || Mbr.IsMbr(disk))
        {
            Console.WriteLine(
                "FS: A partition table already exists");
            return false;
        }

        switch (kind)
        {
            case PartitionTableKind.GPT:
                Gpt.Create(disk);
                break;

            case PartitionTableKind.MBR:
                Mbr.Create(disk);
                break;

            default:
                Console.WriteLine(
                    "FS: Unsupported partition table type");
                return false;
        }

        StorageManager.RescanPartitions(disk);
        Console.WriteLine($"FS: Initialized {kind}");
        return true;
    }

    // The caller must supply a valid, non-overlapping free range.
    public bool CreatePartition(
    int diskIndex,
    ulong startSector,
    ulong sectorCount,
    byte mbrSystemId = 0x0C)
    {
        IBlockDevice? disk = GetDisk(diskIndex);

        if (disk is null)
            return false;

        if (!IsRangeWithinDisk(disk, startSector, sectorCount))
        {
            Console.WriteLine("FS: Invalid partition range");
            return false;
        }

        if (!Gpt.IsGpt(disk) && !Mbr.IsMbr(disk))
        {
            Console.WriteLine(
                "FS: Initialize a partition table first");
            return false;
        }

        if (!PartitionManager.Create(
            disk,
            startSector: startSector,
            sectorCount: sectorCount,
            mbrSystemId: mbrSystemId,
            gptType: Gpt.BasicDataPartitionType))
        {
            Console.WriteLine("FS: Create failed");
            return false;
        }

        StorageManager.RescanPartitions(disk);
        return true;
    }

    public bool DeletePartition(
        int diskIndex,
        ulong startSector,
        ulong sectorCount)
    {
        IBlockDevice? disk = GetDisk(diskIndex);

        if (disk is null)
        {
            Console.WriteLine("FS: No disk detected");
            return false;
        }

        PartitionManager.PartitionLocation location =
            new(startSector, sectorCount);

        if (!PartitionManager.Delete(disk, location))
        {
            Console.WriteLine("FS: Delete failed");
            return false;
        }

        StorageManager.RescanPartitions(disk);
        Console.WriteLine("FS: Partition deleted");
        return true;
    }

    public bool ResizePartition(
        int diskIndex,
        ulong startSector,
        ulong oldSectorCount,
        ulong newSectorCount)
    {
        IBlockDevice? disk = GetDisk(diskIndex);

        if (disk is null)
        {
            Console.WriteLine("FS: No disk detected");
            return false;
        }

        if (newSectorCount == 0)
        {
            Console.WriteLine(
                "FS: Partition size must be greater than zero");
            return false;
        }

        PartitionManager.PartitionLocation location =
            new(startSector, oldSectorCount);

        if (!PartitionManager.Resize(
                disk, location, newSectorCount))
        {
            Console.WriteLine("FS: Resize failed");
            return false;
        }

        StorageManager.RescanPartitions(disk);
        Console.WriteLine(
            $"FS: Resized partition to {newSectorCount} sectors");

        return true;
    }
    private static IBlockDevice? GetDisk(int diskIndex)
    {
        if (diskIndex < 0 ||
            diskIndex >= StorageManager.DeviceCount)
        {
            Console.WriteLine("FS: Invalid disk index");
            return null;
        }

        return StorageManager.GetDevice(diskIndex);
    }

    private static bool IsRangeWithinDisk(
        IBlockDevice disk,
        ulong startSector,
        ulong sectorCount)
    {
        if (sectorCount == 0)
            return false;

        if (startSector >= disk.BlockCount)
            return false;

        // Subtraction avoids overflow in startSector + sectorCount.
        return sectorCount <= disk.BlockCount - startSector;
    }
}

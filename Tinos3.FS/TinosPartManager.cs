
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
            return;

        if (Gpt.IsGpt(disk))
        {
            var partitions = Gpt.Parse(disk);
            Console.WriteLine($"FS: GPT, {partitions.Count} partition(s)");
        }
        else if (Mbr.IsMbr(disk))
        {
            var partitions = Mbr.Parse(disk);
            Console.WriteLine($"FS: MBR, {partitions.Count} partition(s)");
        }
        else
        {
            Console.WriteLine("FS: No recognized partition table");
        }
    }

    // Explicitly initialize a partition table.
    // Never call this automatically during normal startup.
    public bool InitializePartitionTable(
        int diskIndex,
        PartitionTableKind kind)
    {
        IBlockDevice? disk = GetDisk(diskIndex);

        if (disk is null)
            return false;

        if (Gpt.IsGpt(disk) || Mbr.IsMbr(disk))
        {
            Console.WriteLine("FS: A partition table already exists");
            return false;
        }

        try
        {
            switch (kind)
            {
                case PartitionTableKind.GPT:
                    Gpt.Create(disk);
                    break;

                case PartitionTableKind.MBR:
                    Mbr.Create(disk);
                    break;

                default:
                    Console.WriteLine("FS: Unsupported partition table type");
                    return false;
            }

            // Verify that the requested table now exists.
            bool initialized = kind switch
            {
                PartitionTableKind.GPT => Gpt.IsGpt(disk),
                PartitionTableKind.MBR => Mbr.IsMbr(disk),
                _ => false
            };

            if (!initialized)
            {
                Console.WriteLine("FS: Partition table initialization failed");
                return false;
            }

            StorageManager.RescanPartitions(disk);

            Console.WriteLine($"FS: Initialized {kind}");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"FS: Partition table initialization failed: {ex.Message}");
            return false;
        }
    }

    // Creates a partition at the requested location.
    // The caller must supply a free, non-overlapping range that is valid
    // for the selected partition table.
    public bool CreatePartition(
        int diskIndex,
        ulong startSector,
        ulong sectorCount,
        byte mbrSystemId = 0x0C)
    {
        IBlockDevice? disk = GetDisk(diskIndex);

        if (disk is null)
            return false;

        if (!Gpt.IsGpt(disk) && !Mbr.IsMbr(disk))
        {
            Console.WriteLine(
                "FS: Initialize a partition table first");
            return false;
        }

        if (!IsRangeWithinDisk(disk, startSector, sectorCount))
        {
            Console.WriteLine(
                "FS: Invalid partition range or range exceeds disk boundaries");
            return false;
        }

        try
        {
            bool success = PartitionManager.Create(
                disk,
                startSector: startSector,
                sectorCount: sectorCount,
                mbrSystemId: mbrSystemId,
                gptType: Gpt.BasicDataPartitionType);

            if (!success)
            {
                Console.WriteLine(
                    "FS: Partition creation failed");
                return false;
            }

            StorageManager.RescanPartitions(disk);

            Console.WriteLine(
                $"FS: Created partition at sector {startSector}, " +
                $"size {sectorCount} sectors");

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"FS: Partition creation failed: {ex.Message}");
            return false;
        }
    }

    public bool DeletePartition(
        int diskIndex,
        ulong startSector,
        ulong sectorCount)
    {
        IBlockDevice? disk = GetDisk(diskIndex);

        if (disk is null)
            return false;

        if (!IsRangeWithinDisk(disk, startSector, sectorCount))
        {
            Console.WriteLine("FS: Invalid partition range");
            return false;
        }

        try
        {
            PartitionManager.PartitionLocation location =
                new(startSector, sectorCount);

            if (!PartitionManager.Delete(disk, location))
            {
                Console.WriteLine("FS: Partition deletion failed");
                return false;
            }

            StorageManager.RescanPartitions(disk);

            Console.WriteLine("FS: Partition deleted");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"FS: Partition deletion failed: {ex.Message}");
            return false;
        }
    }

    public bool ResizePartition(
        int diskIndex,
        ulong startSector,
        ulong oldSectorCount,
        ulong newSectorCount)
    {
        IBlockDevice? disk = GetDisk(diskIndex);

        if (disk is null)
            return false;

        if (!IsRangeWithinDisk(disk, startSector, oldSectorCount))
        {
            Console.WriteLine("FS: Invalid existing partition range");
            return false;
        }

        if (!IsRangeWithinDisk(disk, startSector, newSectorCount))
        {
            Console.WriteLine(
                "FS: New partition size exceeds disk boundaries");
            return false;
        }

        try
        {
            PartitionManager.PartitionLocation location =
                new(startSector, oldSectorCount);

            if (!PartitionManager.Resize(
                    disk, location, newSectorCount))
            {
                Console.WriteLine("FS: Partition resize failed");
                return false;
            }

            StorageManager.RescanPartitions(disk);

            Console.WriteLine(
                $"FS: Resized partition to {newSectorCount} sectors");

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"FS: Partition resize failed: {ex.Message}");
            return false;
        }
    }

    // Checks physical disk boundaries without overflowing ulong.
    private static bool IsRangeWithinDisk(
        IBlockDevice disk,
        ulong startSector,
        ulong sectorCount)
    {
        return sectorCount > 0
            && startSector > 0
            && startSector < disk.BlockCount
            && sectorCount <= disk.BlockCount - startSector;
    }

    private static IBlockDevice? GetDisk(int diskIndex)
    {
        if (diskIndex < 0 ||
            diskIndex >= StorageManager.DeviceCount)
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

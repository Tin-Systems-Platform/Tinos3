using Cosmos.Kernel.HAL.Devices.Storage;
using Cosmos.Kernel.System.Storage;

namespace Tinos3.FS;

public class DiskMgr
{
    public int GetDiskCount()
    {
        return StorageManager.DeviceCount;
    }

    public void ListDisks()
    {
        for (int i = 0; i < StorageManager.DeviceCount; i++)
        {
            IBlockDevice? disk = StorageManager.GetDevice(i);

            if (disk is null)
                continue;

            ulong sizeBytes = checked(disk.BlockCount * disk.BlockSize);

            Console.WriteLine($"Disk {i}: {disk.Name}");
            Console.WriteLine($"  Blocks: {disk.BlockCount}");
            Console.WriteLine($"  Block size: {disk.BlockSize} bytes");
            Console.WriteLine($"  Capacity: {sizeBytes} bytes");
            Console.WriteLine($"  Type: {(Gpt.IsGpt(disk) ? "GPT" : Mbr.IsMbr(disk) ? "MBR" : "Unknown")}");
        }
    }
}
using System.IO;
using Cosmos.Kernel.System.Storage;
using Cosmos.Kernel.System.Vfs;
using Cosmos.Kernel.System.Filesystems.Fat;
using Cosmos.Kernel.HAL.Interfaces.Devices;
using Cosmos.Kernel.HAL.Vfs;
using System.ComponentModel.DataAnnotations;

namespace Tinos3.FileSystem
{
    internal class TinosPartitionManager
    {
        public void listPartitions()
        {
            IBlockDevice? disk = StorageManager.PrimaryDevice;
            if (disk is null)
            {
                Console.WriteLine("FS: No disk detected");
                return;
            }

            if (Gpt.IsGpt(disk))
            {
                Console.WriteLine("FS: GPT, " + Gpt.Parse(disk).Count + " partition(s)");
            }
            else if (Mbr.IsMbr(disk))
            {
                Console.WriteLine("FS: MBR, " + Mbr.Parse(disk).Count + " partition(s)");
            }
            else
            {
                Console.WriteLine("FS: No partition table");
            }
        }

        public void createPartition(ulong sectorCountForPart, string partitionName)
        {
            IBlockDevice? disk = StorageManager.PrimaryDevice;

            Gpt.Create(disk);



            if (!PartitionManager.Create(disk, startSector: 2048, sectorCount: sectorCountForPart,
                                         mbrSystemId: 0x0C, gptType: Gpt.BasicDataPartitionType))
            {
                Console.WriteLine("FS: Create failed");
                return;
            }

            StorageManager.RescanPartitions(disk);

            Console.WriteLine("FS: Created partition " + partitionName + " with " + sectorCountForPart + " sectors");
        }

        public void deletePartition(string partitionName, ulong sectorCount)
        {
            PartitionManager.PartitionLocation partLoc = new(startSector: 2048, sectorCount: sectorCount);

            IBlockDevice? disk = StorageManager.PrimaryDevice;
            if (!PartitionManager.Delete(disk, partLoc))
            {
                Console.WriteLine("FS: Delete failed");
                return;
            }
            StorageManager.RescanPartitions(disk);
            Console.WriteLine("FS: Deleted partition " + partitionName);
        }

        public void resizePartition(string partitionName, ulong newSectorCount, ulong oldSectorCount)
        {
            PartitionManager.PartitionLocation partLoc = new(startSector: 2048, sectorCount: oldSectorCount);
            IBlockDevice? disk = StorageManager.PrimaryDevice;
            if (!PartitionManager.Resize(disk, partLoc, newSectorCount))
            {
                Console.WriteLine("FS: Resize failed");
                return;
            }
            StorageManager.RescanPartitions(disk);
            Console.WriteLine("FS: Resized partition " + partitionName + " to " + newSectorCount + " sectors");
        }

        public void getFreespaceOnPartition(int partitionIndex, int diskIndex, string mountPoint) {
            IBlockDevice? disk = StorageManager.GetDevice(diskIndex);

            if (VfsManager.TryStatFs("/" + mountPoint, out VfsStatFs stats))
            {
                ulong freeBytes = stats.Bavail * stats.BlockSize;
                ulong totalBytes = stats.Blocks * stats.BlockSize;
                
                ulong freeMB = freeBytes / (1024 * 1024);
                ulong totalMB = totalBytes / (1024 * 1024);

                Console.WriteLine($"FS: Partition {partitionIndex} on disk {diskIndex} mounted at /{mountPoint} has {freeMB} MB free out of {totalMB} MB total.");
            }
        }
    }
}

using Cosmos.Kernel.System.FileSystem;
using Cosmos.Kernel.System.FileSystem.Fat;
using Cosmos.Kernel.System.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using Cosmos.Kernel.System.FileSystem.Iso9660;
using Cosmos.Kernel.HAL.Devices.Storage;

namespace Tinos3.FS
{
    public class RegisterFSActions
    {
        public static void InitFS()
        {
            Console.WriteLine("FS: Initializing filesystem subsystem.");
            Console.WriteLine("FS: Finding CDROM partitions...");

            IBlockDevice cdrom = null;

            foreach(IBlockDevice device in StorageManager.Devices)
            {
                if (device is IRemovableMedia { IsReadOnly: true})
                {
                    cdrom = device;
                    break;
                }
            }

            FatFileSystemType fat = new();

            if (!VfsManager.RegisterFileSystem("fat", fat))
            {
                Console.WriteLine("FS: Filesystem 'fat' is already registered.");
            }

            if (StorageManager.Partitions.Count == 0)
            {
                Console.WriteLine("FS: No partitions found.");
                return;
            }

            Console.WriteLine(
                $"FS: Found {StorageManager.Partitions.Count} partition(s).");

            Console.WriteLine("FS: Attempting to mount partition 0...");

            if (VfsManager.TryMount("fat", StorageManager.Partitions[0], MountFlags.None, "/mnt", out VfsMount? mount))
            {
                Console.WriteLine("Mounted " + mount.Name + " at " + mount.MountPoint);
            }
            else
            {
                Console.WriteLine("FS: Failed to mount partition 0.");
            }

            Console.WriteLine(
                "FS: Filesystem initialization complete.");
        }
    }
}

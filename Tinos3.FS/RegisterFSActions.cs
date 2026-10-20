using Cosmos.Kernel.System.FileSystem;
using Cosmos.Kernel.System.FileSystem.Fat;
using Cosmos.Kernel.System.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tinos3.FS
{
    public class RegisterFSActions
    {
        public static void InitFS()
        {
            Console.WriteLine("FS: Initializing filesystem subsystem.");

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

            Console.WriteLine(
                "FS: Filesystem initialization complete.");
        }
    }
}

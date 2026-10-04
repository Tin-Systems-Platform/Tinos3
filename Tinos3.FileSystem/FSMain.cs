using System.IO;
using Cosmos.Kernel.System.Storage;
using Cosmos.Kernel.System.Vfs;
using Cosmos.Kernel.System.Filesystems.Fat;
using Cosmos.Kernel.HAL.Interfaces.Devices;
using Cosmos.Kernel.HAL.Vfs;


namespace Tinos3.FileSystem
{
    public class FSMain
    {
        public static void initFS()
        {

            Console.WriteLine("FS: Initializing Filesystems and mounting disk to /mnt");

            FatFilesystemType fat = new();

            if (!VfsManager.RegisterFilesystem("fat", fat))
            {
                Console.WriteLine("FS: The name \"fat\" is already registered.");
                return;
            }

            if (StorageManager.Partitions.Count == 0)
            {
                Console.WriteLine("FS: No partitions found.");
                return;
            }
            // TODO: Multiple partition, currently we are using StorageManager.Partitions[0] as the first partition to mount
            //       we need to implement a way to select which partition to mount.
            if (VfsManager.TryMount("fat", StorageManager.Partitions[0], MountFlags.None, "/mnt", out VfsManager.VfsMount? mount))
            {
                Console.WriteLine("Mounted " + mount.Name + " at " + mount.MountPoint);
            }
            else
            {
                Console.WriteLine("FS: Failed to mount partition.");
            }

            Console.WriteLine("FS: Filesystem init complete. User space commands can now run filesystem related stuff.");
        }
    }
}

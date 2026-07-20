using Cosmos.Kernel.HAL.Vfs;
using Cosmos.Kernel.System.Filesystems.Fat;
using Cosmos.Kernel.System.Vfs;

namespace Tinos3.FileSystem
{
    public class FSMain
    {
        public static void initFS()
        {
            Console.WriteLine("FS: Initializing Filesystems and mounting disk to /mnt");

            FatFilesystemType fat = new();

            VfsManager.RegisterFilesystem("fat", fat);
            
            if (VfsManager.TryMount("fat", "0", MountFlags.None, "/mnt", out VfsManager.VfsMount? mount))
            {
                Console.WriteLine("Mounted " + mount.Name + " partition " + mount.Source + " at " + mount.MountPoint);
            }

            Console.WriteLine("FS: Filesystem init complete. User space commands can now run filesystem related stuff.");
        }
    }
}

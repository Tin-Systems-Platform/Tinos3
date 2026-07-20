using Cosmos.Kernel.HAL.Vfs;
using Cosmos.Kernel.System.Filesystems.Fat;
using Cosmos.Kernel.System.Vfs;
using System;
using System.Collections.Generic;
using System.Text;
using Tinos3.Abstractions.Commands;

namespace Tinos3.FileSystem.Commands
{
    public class FormatCmd : Command
    {
        public FormatCmd(string name) : base(name)
        {
        }

        public override string Execute(string[] args)
        {
            formatLogic();

            return "";
        }

        private void formatLogic()
        {
            Console.WriteLine("Unmounting partion for formatting purposes as formatting is refused if it is mounted");

            VfsManager.TryUnmount("/mnt");

            FatFormatOptions options = new()
            {
                Type = FatType.Fat32,
                VolumeLabel = "TINOS     ",
            };

            if (!VfsManager.TryFormat("fat", "0", options))
            {
                Console.WriteLine("Format failed");
                return;
            }

            Console.WriteLine("Mounting the partion back.");

            if (VfsManager.TryMount("fat", "0", MountFlags.None, "/mnt", out VfsManager.VfsMount? mount))
            {
                Console.WriteLine("Mounted " + mount.Name + " partition " + mount.Source + " at " + mount.MountPoint);
            }

            Console.WriteLine("Formatting complete.");
        }
    }
}

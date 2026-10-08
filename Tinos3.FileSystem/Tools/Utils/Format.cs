using System.IO;
using Cosmos.Kernel.HAL.Devices.Storage;
using Cosmos.Kernel.System.FileSystem;
using Cosmos.Kernel.System.FileSystem.Fat;
using Cosmos.Kernel.System.Storage;

namespace Tinos3.FileSystem.Tools.Utils
{
    public class Format
    {
        public void formatPartition()
        {
            IBlockDevice? disk = StorageManager.PrimaryDevice;
            if (disk is null)
            {
                Console.WriteLine("FS: No primary device found");
                return;
            }

            Console.WriteLine("Welcome to the Format utility");
            Console.WriteLine("Type help for a list of available commands");

            while (true)
            {
                Console.Write("format> ");
                var input = Console.ReadLine();

                if (string.IsNullOrEmpty(input))
                    return;

                switch (input.ToLower())
                {
                    case "help":
                        Console.WriteLine("Available commands:");
                        Console.WriteLine("  help     - Show this help message");
                        Console.WriteLine("  format   - Format the partition");
                        Console.WriteLine("  exit     - Exit the format utility");
                        break;
                    case "format":
                        Console.WriteLine("What partition do you want to format? (Enter the partition number, e.g., 0 for the first partition)");
                        var partitionInput = Console.ReadLine();
                        if (string.IsNullOrEmpty(partitionInput))
                            Console.WriteLine("Invalid input. Please enter a valid partition number.");
                        else if (!int.TryParse(partitionInput, out int partitionNumberIndex) || partitionNumberIndex < 0 || partitionNumberIndex >= StorageManager.Partitions.Count)
                            Console.WriteLine("Invalid partition number.");
                        else
                        {
                            Console.WriteLine("Enter a label for the partition:");
                            var partitionName = Console.ReadLine();

                            if (string.IsNullOrEmpty(partitionName))
                            {
                                Console.WriteLine("Invalid partition name.");
                                return;
                            }

                            formatPartitionLogic(partitionName, partitionNumberIndex);
                        }
                        return;
                    case "exit":
                        return;
                    default:
                        Console.WriteLine($"\"{input}\" is not a command");
                        break;
                }
            }
        }

        private void formatPartitionLogic(string partitionName,int partitionNumberIndex)
        {
            Console.WriteLine("Unmounting partion for formatting purposes as formatting is refused if it is mounted");

            VfsManager.TryUnmount("/mnt");

            FatFormatOptions options = new()
            {
                Type = FatType.Fat32,
                VolumeLabel = partitionName,
            };


            if (StorageManager.Partitions.Count == 0
                || !VfsManager.TryFormat("fat", StorageManager.Partitions[partitionNumberIndex], options))
            {
                Console.WriteLine("Format failed");
            }

            Console.WriteLine("Mounting the partion back.");

            if (VfsManager.TryMount("fat", StorageManager.Partitions[partitionNumberIndex], MountFlags.None, "/mnt", out VfsMount? mount))
            {
                Console.WriteLine("Mounted " + mount.Name + " partition " + mount.Source + " at " + mount.MountPoint);
            }

            Console.WriteLine("Formatting complete.");
        }
    }
}

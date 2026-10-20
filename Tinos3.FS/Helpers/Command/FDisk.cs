
using Cosmos.Executable.Lua;
using Cosmos.Kernel.System.FileSystem;
using Cosmos.Kernel.System.FileSystem.Fat;
using Cosmos.Kernel.System.Storage;
using System;
using Tinos3.FS;

namespace Tinos3.FS.Helpers.Command
{
    public class FDisk
    {
        public void FDiskMain()
        {
            var partitionManager = new TinosPartManager();

            Console.WriteLine("Tinos3 FDisk");
            Console.WriteLine("Type 'help' to list commands.");

            while (true)
            {
                Console.Write("FDisk> ");

                string? input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                    continue;

                string[] commandArgs = input.Split(
                    ' ',
                    StringSplitOptions.RemoveEmptyEntries);

                string command = commandArgs[0].ToLowerInvariant();

                switch (command)
                {
                    case "create":
                        {
                            if (commandArgs.Length != 4)
                            {
                                Console.WriteLine(
                                    "Usage: create <diskIndex> <startSector> <sizeInSectors>");
                                break;
                            }

                            if (!int.TryParse(commandArgs[1], out int diskIndex) ||
                                diskIndex < 0)
                            {
                                Console.WriteLine("Invalid disk index.");
                                break;
                            }

                            if (!ulong.TryParse(commandArgs[2], out ulong startSector) ||
                                !ulong.TryParse(commandArgs[3], out ulong sizeInSectors) ||
                                startSector == 0 ||
                                sizeInSectors == 0)
                            {
                                Console.WriteLine(
                                    "Invalid start sector or partition size.");
                                break;
                            }

                            bool success = partitionManager.CreatePartition(
                                diskIndex,
                                startSector,
                                sizeInSectors);

                            Console.WriteLine(success
                                ? "Partition created successfully."
                                : "Partition creation failed.");

                            break;
                        }

                    case "delete":
                        {
                            if (commandArgs.Length != 4)
                            {
                                Console.WriteLine(
                                    "Usage: delete <diskIndex> <startSector> <sizeInSectors>");
                                break;
                            }

                            if (!int.TryParse(commandArgs[1], out int diskIndex) ||
                                diskIndex < 0)
                            {
                                Console.WriteLine("Invalid disk index.");
                                break;
                            }

                            if (!ulong.TryParse(commandArgs[2], out ulong startSector) ||
                                !ulong.TryParse(commandArgs[3], out ulong sizeInSectors) ||
                                startSector == 0 ||
                                sizeInSectors == 0)
                            {
                                Console.WriteLine(
                                    "Invalid start sector or partition size.");
                                break;
                            }

                            Console.Write(
                                $"Delete partition at sector {startSector}, " +
                                $"size {sizeInSectors} sectors? (yes/no): ");

                            if (!Confirm())
                            {
                                Console.WriteLine("Deletion cancelled.");
                                break;
                            }

                            bool success = partitionManager.DeletePartition(
                                diskIndex,
                                startSector,
                                sizeInSectors);

                            Console.WriteLine(success
                                ? "Partition deleted successfully."
                                : "Partition deletion failed.");

                            break;
                        }

                    case "resize":
                        {
                            if (commandArgs.Length != 5)
                            {
                                Console.WriteLine(
                                    "Usage: resize <diskIndex> <startSector> " +
                                    "<newSizeInSectors> <oldSizeInSectors>");
                                break;
                            }

                            if (!int.TryParse(commandArgs[1], out int diskIndex) ||
                                diskIndex < 0)
                            {
                                Console.WriteLine("Invalid disk index.");
                                break;
                            }

                            if (!ulong.TryParse(commandArgs[2], out ulong startSector) ||
                                !ulong.TryParse(commandArgs[3], out ulong newSizeInSectors) ||
                                !ulong.TryParse(commandArgs[4], out ulong oldSizeInSectors) ||
                                startSector == 0 ||
                                newSizeInSectors == 0 ||
                                oldSizeInSectors == 0)
                            {
                                Console.WriteLine(
                                    "Invalid sector number or partition size.");
                                break;
                            }

                            Console.Write(
                                $"Resize partition at sector {startSector} " +
                                $"from {oldSizeInSectors} to {newSizeInSectors} " +
                                "sectors? (yes/no): ");

                            if (!Confirm())
                            {
                                Console.WriteLine("Resize cancelled.");
                                break;
                            }

                            // The manager expects OLD size before NEW size.
                            bool success = partitionManager.ResizePartition(
                                diskIndex,
                                startSector,
                                oldSizeInSectors,
                                newSizeInSectors);

                            Console.WriteLine(success
                                ? "Partition resized successfully."
                                : "Partition resize failed.");

                            break;
                        }

                    case "listpartitions":
                        {
                            if (commandArgs.Length != 2)
                            {
                                Console.WriteLine(
                                    "Usage: listpartitions <diskIndex>");
                                break;
                            }

                            if (!int.TryParse(commandArgs[1], out int diskIndex) ||
                                diskIndex < 0)
                            {
                                Console.WriteLine("Invalid disk index.");
                                break;
                            }

                            partitionManager.ListPartitions(diskIndex);
                            break;
                        }

                    case "init":
                        {
                            if (commandArgs.Length != 3)
                            {
                                Console.WriteLine(
                                    "Usage: init <diskIndex> <MBR|GPT>");
                                break;
                            }

                            if (!int.TryParse(commandArgs[1], out int diskIndex) ||
                                diskIndex < 0)
                            {
                                Console.WriteLine("Invalid disk index.");
                                break;
                            }

                            if (!Enum.TryParse(
                                commandArgs[2],
                                true,
                                out PartitionTableKind kind) ||
                                (kind != PartitionTableKind.MBR &&
                                 kind != PartitionTableKind.GPT))
                            {
                                Console.WriteLine(
                                    "Invalid partition table kind. Use MBR or GPT.");
                                break;
                            }

                            Console.WriteLine(
                                $"WARNING: Initializing {kind} may overwrite " +
                                "existing disk metadata.");

                            Console.Write("Continue? (yes/no): ");

                            if (!Confirm())
                            {
                                Console.WriteLine("Initialization cancelled.");
                                break;
                            }

                            bool success =
                                partitionManager.InitializePartitionTable(
                                    diskIndex,
                                    kind);

                            Console.WriteLine(success
                                ? $"Initialized {kind} successfully."
                                : "Partition table initialization failed.");

                            break;
                        }

                    case "listdisks":
                        {
                            Console.WriteLine("Available disks:");
                            new DiskMgr().ListDisks();
                            break;
                        }

                    case "help":
                        {
                            Console.WriteLine("Available commands:");
                            Console.WriteLine(
                                "  create <diskIndex> <startSector> <sizeInSectors>");
                            Console.WriteLine(
                                "  delete <diskIndex> <startSector> <sizeInSectors>");
                            Console.WriteLine(
                                "  resize <diskIndex> <startSector> <newSizeInSectors> <oldSizeInSectors>");
                            Console.WriteLine(
                                "  listpartitions <diskIndex>");
                            Console.WriteLine(
                                "  init <diskIndex> <MBR|GPT>");
                            Console.WriteLine("  listdisks");
                            Console.WriteLine("  format <diskIndex> <partitionIndex> <filesystemType>");
                            Console.WriteLine("  help");
                            Console.WriteLine("  exit");
                            break;
                        }

                    case "format":
                        {
                            

                            if (commandArgs.Length != 4)
                            {
                                Console.WriteLine(
                                    "Usage: format <diskIndex> <partitionIndex> <filesystemType>");
                                break;
                            }

                            // Check if the partition exists and isn't mounted before formatting. For simplicity, we assume the partition exists.
                            if (!VfsManager.TryUnmount("/mnt"))
                            {
                                Console.WriteLine("Failed to unmount the partition. Partition isn't mounted.");
                                break;
                            }

                            if (!int.TryParse(commandArgs[1], out int diskIndex) ||
                                diskIndex < 0)
                            {
                                Console.WriteLine("Invalid disk index.");
                                break;
                            }

                            if (!int.TryParse(commandArgs[2], out int partitionIndex) ||
                                partitionIndex < 0 ||
                                partitionIndex >= StorageManager.Partitions.Count)
                            {
                                Console.WriteLine("Invalid partition index.");
                                break;
                            }

                            string filesystemType = commandArgs[3];

                            if (!filesystemType.Equals(
                                "fat32",
                                StringComparison.OrdinalIgnoreCase))
                            {
                                Console.WriteLine(
                                    $"Unsupported filesystem type: {filesystemType}");
                                Console.WriteLine("Currently supported: FAT32");
                                break;
                            }

                            Console.WriteLine(
                                $"WARNING: Formatting partition {partitionIndex} " +
                                $"on disk {diskIndex} will erase its data.");

                            Console.Write("Continue? (yes/no): ");

                            if (!Confirm())
                            {
                                Console.WriteLine("Format cancelled.");
                                break;
                            }

                            FatFormatOptions options = new()
                            {
                                Type = FatType.Fat32,
                                VolumeLabel = "TINOS3"
                            };

                            bool success = VfsManager.TryFormat(
                                "fat",
                                StorageManager.Partitions[partitionIndex],
                                options);

                            Console.WriteLine(success
                                ? $"Partition {partitionIndex} formatted as FAT32 successfully."
                                : "Partition formatting failed.");

                            break;
                        }


                    case "exit":
                        Console.WriteLine("Exiting FDisk.");
                        return;

                    default:
                        Console.WriteLine($"Unknown command: {command}");
                        Console.WriteLine("Type 'help' to list commands.");
                        break;
                }
            }
        }

        private static bool Confirm()
        {
            string? confirmation = Console.ReadLine();

            return string.Equals(
                confirmation?.Trim(),
                "yes",
                StringComparison.OrdinalIgnoreCase);
        }

        public static int LuaFDiskMain(ILuaState lua)
        {
            var fDisk = new FDisk();
            fDisk.FDiskMain();
            return 0;
        }
    }
}

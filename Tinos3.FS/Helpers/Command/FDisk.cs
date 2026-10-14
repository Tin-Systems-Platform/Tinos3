using Cosmos.Executable.Lua;
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
                {
                    continue;
                }

                string[] commandArgs = input.Split(
                    ' ',
                    StringSplitOptions.RemoveEmptyEntries
                );

                string command = commandArgs[0].ToLowerInvariant();

                switch (command)
                {
                    case "create":
                        {
                            if (commandArgs.Length != 4)
                            {
                                Console.WriteLine(
                                    "Usage: create <diskIndex> <startSector> <sizeInSectors>"
                                );
                                break;
                            }

                            if (!int.TryParse(commandArgs[1], out int diskIndex) ||
                                diskIndex < 0)
                            {
                                Console.WriteLine("Invalid disk index.");
                                break;
                            }

                            if (!ulong.TryParse(commandArgs[2], out ulong startSector) ||
                                !ulong.TryParse(commandArgs[3], out ulong sizeInSectors))
                            {
                                Console.WriteLine("Invalid sector number or size.");
                                break;
                            }

                            if (sizeInSectors == 0)
                            {
                                Console.WriteLine("Partition size must be greater than zero.");
                                break;
                            }

                            partitionManager.CreatePartition(
                                diskIndex,
                                startSector,
                                sizeInSectors
                            );

                            break;
                        }

                    case "delete":
                        {
                            if (commandArgs.Length != 4)
                            {
                                Console.WriteLine(
                                    "Usage: delete <diskIndex> <startSector> <sizeInSectors>"
                                );
                                break;
                            }

                            if (!int.TryParse(commandArgs[1], out int diskIndex) ||
                                diskIndex < 0)
                            {
                                Console.WriteLine("Invalid disk index.");
                                break;
                            }

                            if (!ulong.TryParse(commandArgs[2], out ulong startSector) ||
                                !ulong.TryParse(commandArgs[3], out ulong sizeInSectors))
                            {
                                Console.WriteLine("Invalid sector number or size.");
                                break;
                            }

                            if (sizeInSectors == 0)
                            {
                                Console.WriteLine("Partition size must be greater than zero.");
                                break;
                            }

                            Console.Write(
                                $"Delete partition starting at sector {startSector}? (yes/no): "
                            );

                            string? confirmation = Console.ReadLine();

                            if (!string.Equals(
                                confirmation?.Trim(),
                                "yes",
                                StringComparison.OrdinalIgnoreCase))
                            {
                                Console.WriteLine("Deletion cancelled.");
                                break;
                            }

                            partitionManager.DeletePartition(
                                diskIndex,
                                startSector,
                                sizeInSectors
                            );

                            break;
                        }

                    case "resize":
                        {
                            if (commandArgs.Length != 5)
                            {
                                Console.WriteLine(
                                    "Usage: resize <diskIndex> <startSector> <newSizeInSectors> <oldSizeInSectors>"
                                );
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
                                !ulong.TryParse(commandArgs[4], out ulong oldSizeInSectors))
                            {
                                Console.WriteLine("Invalid sector number or size.");
                                break;
                            }

                            if (newSizeInSectors == 0 || oldSizeInSectors == 0)
                            {
                                Console.WriteLine("Partition sizes must be greater than zero.");
                                break;
                            }

                            Console.Write(
                                $"Resize partition at sector {startSector} " +
                                $"from {oldSizeInSectors} to {newSizeInSectors} sectors? (yes/no): "
                            );

                            string? confirmation = Console.ReadLine();

                            if (!string.Equals(
                                confirmation?.Trim(),
                                "yes",
                                StringComparison.OrdinalIgnoreCase))
                            {
                                Console.WriteLine("Resize cancelled.");
                                break;
                            }

                            partitionManager.ResizePartition(
                                diskIndex,
                                startSector,
                                newSizeInSectors,
                                oldSizeInSectors
                            );

                            break;
                        }

                    case "help":
                        Console.WriteLine("Available commands:");
                        Console.WriteLine(
                            "  create <diskIndex> <startSector> <sizeInSectors>"
                        );
                        Console.WriteLine(
                            "  delete <diskIndex> <startSector> <sizeInSectors>"
                        );
                        Console.WriteLine(
                            "  resize <diskIndex> <startSector> <newSizeInSectors> <oldSizeInSectors>"
                        );
                        Console.WriteLine("  help");
                        Console.WriteLine("  exit");
                        Console.WriteLine("  listpartitions <diskIndex>");
                        Console.WriteLine("  init <diskIndex> <partitionTableKind>");
                        Console.WriteLine("  listdisks");
                        break;
                    case "listpartitions":
                        {
                            if (commandArgs.Length != 2)
                            {
                                Console.WriteLine("Usage: list <diskIndex>");
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
                                    "Usage: init <diskIndex> <partitionTableKind>"
                                );
                                break;
                            }
                            if (!int.TryParse(commandArgs[1], out int diskIndex) ||
                                diskIndex < 0)
                            {
                                Console.WriteLine("Invalid disk index.");
                                break;
                            }
                            if (!Enum.TryParse<PartitionTableKind>(
                                commandArgs[2],
                                true,
                                out var kind))
                            {
                                Console.WriteLine(
                                    "Invalid partition table kind. Use 'MBR' or 'GPT'."
                                );
                                break;
                            }
                            partitionManager.InitializePartitionTable(diskIndex, kind);
                            break;
                        }
                    case "listdisks":
                        {
                            Console.WriteLine("Available disks:");
                            DiskMgr mgr = new DiskMgr();
                            mgr.ListDisks();
                            break;
                        }
                    case "exit":
                        Console.WriteLine("Exiting FDisk.");
                        return;

                    default:
                        Console.WriteLine($"Unknown command: {command}");
                        Console.WriteLine("Type 'help' for available commands.");
                        break;
                }
            }
        }

        public static int LuaFDiskMain(ILuaState lua)
        {
            FDisk fDisk = new FDisk();
            fDisk.FDiskMain();
            return 0; // No return values to Lua
        }
    }
}
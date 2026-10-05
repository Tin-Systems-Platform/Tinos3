using System;
using System.Collections.Generic;
using System.Text;
using Tinos3.Abstractions.Commands;

// Inlcude the classes for partition management and storage management
using Tinos3.FileSystem;

namespace Tinos3.FileSystem.Tools
{
    public class FDisk : Command
    {
        public FDisk(string name) : base(name)
        {
        }

        public override string Execute(string[] args)
        {
            var partitionManager = new TinosPartitionManager();
            while (true)
            {
                Console.Write("FDisk> ");
                string input = Console.ReadLine();
                if (input == null)
                {
                    continue;
                }
                string[] commandArgs = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (commandArgs.Length == 0)
                {
                    continue;
                }
                string command = commandArgs[0];
                switch (command)
                {
                    case "create":
                        if (commandArgs.Length < 3)
                        {
                            Console.WriteLine("Usage: create <partitionName> <sizeInSectors>");
                            break;
                        }
                        {
                            string partitionName = commandArgs[1];
                            if (!ulong.TryParse(commandArgs[2], out ulong sizeInSectors))
                            {
                                Console.WriteLine("Invalid size. Please enter a valid number.");
                                break;
                            }
                            partitionManager.createPartition(sizeInSectors, partitionName);
                        }
                        break;
                    case "delete":
                        if (commandArgs.Length < 2)
                        {
                            Console.WriteLine("Usage: delete <partitionName> [sizeInSectors]");
                            break;
                        }
                        {
                            string partitionName = commandArgs[1];
                            ulong sizeForDelete = 0UL;
                            if (commandArgs.Length >= 3)
                            {
                                if (!ulong.TryParse(commandArgs[2], out sizeForDelete))
                                {
                                    Console.WriteLine("Invalid size. Please enter a valid number.");
                                    break;
                                }
                            }
                            partitionManager.deletePartition(partitionName, sizeForDelete);
                        }
                        break;
                    case "resize":
                        if (commandArgs.Length < 4)
                        {
                            Console.WriteLine("Usage: resize <partitionName> <newSizeInSectors> <oldSizeInSectors>");
                            break;
                        }
                        {
                            string partitionName = commandArgs[1];
                            if (!ulong.TryParse(commandArgs[2], out ulong newSizeInSectors) ||
                                !ulong.TryParse(commandArgs[3], out ulong oldSizeInSectors))
                            {
                                Console.WriteLine("Invalid size. Please enter valid numbers.");
                                break;
                            }
                            partitionManager.resizePartition(partitionName, newSizeInSectors, oldSizeInSectors);
                        }
                        break;
                    case "exit":
                        return "";
                    default:
                        Console.WriteLine($"Unknown command: {command}");
                        break;
                }
            }
        }
    }
}

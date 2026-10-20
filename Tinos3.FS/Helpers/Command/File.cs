
using System;
using System.IO;

namespace Tinos3.FS.Helpers.Command
{
    public class File
    {
        private const string MountPoint = "/mnt";

        /// <summary>
        /// Lists files and directories in the specified path.
        /// Relative paths are resolved under /mnt.
        /// </summary>
        public static void ListFiles(string path, bool hasColor = false)
        {
            if (string.IsNullOrWhiteSpace(path))
                path = ".";

            string fullPath = Path.IsPathRooted(path)
                ? Path.GetFullPath(path)
                : Path.GetFullPath(Path.Combine(MountPoint, path));

            if (!Directory.Exists(fullPath))
            {
                Console.WriteLine($"Directory not found: {fullPath}");
                return;
            }

            try
            {
                string[] directories = Directory.GetDirectories(fullPath);
                string[] files = Directory.GetFiles(fullPath);

                ConsoleColor previousColor = Console.ForegroundColor;

                try
                {
                    if (hasColor)
                        Console.ForegroundColor = ConsoleColor.DarkBlue;

                    foreach (string dir in directories)
                        Console.Write(Path.GetFileName(dir));
                        Console.Write(" "); // Add spacing between entries

                    if (hasColor)
                        Console.ForegroundColor = ConsoleColor.Cyan;

                    
                    foreach (string file in files)
                        Console.Write(Path.GetFileName(file));
                        Console.Write(" "); // Add spacing between entries
                }
                finally
                {
                    Console.ForegroundColor = previousColor;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unable to list directory: {ex.Message}");
            }
        }


            /// <summary>
            /// Prints the contents of a file, similar to the Unix cat command.
            /// Relative paths are resolved under /mnt.
            /// </summary>
        public static void ReadFile(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                Console.WriteLine("No file specified.");
                return;
            }

            string fullPath = ResolvePath(path);

            if (!System.IO.File.Exists(fullPath))
            {
                Console.WriteLine($"File '{path}' doesn't exist.");
                return;
            }

            try
            {
                Console.Write(System.IO.File.ReadAllText(fullPath));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unable to read file '{path}': {ex.Message}");
            }
        }

        /// <summary>
        /// Copies a file to a destination.
        /// Supports -o to overwrite an existing file.
        /// </summary>
        public static void CopyFile(string source, string destination, bool overwrite = false)
        {
            if (string.IsNullOrWhiteSpace(source) ||
                string.IsNullOrWhiteSpace(destination))
            {
                Console.WriteLine("No source or destination specified.");
                return;
            }

            string sourcePath = ResolvePath(source);
            string destinationPath = ResolvePath(destination);

            if (!System.IO.File.Exists(sourcePath))
            {
                Console.WriteLine($"Source file '{source}' doesn't exist.");
                return;
            }

            try
            {
                System.IO.File.Copy(sourcePath, destinationPath, overwrite);
                Console.WriteLine($"Copied '{source}' to '{destination}'.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Copy failed: {ex.Message}");
            }
        }

        private static string ResolvePath(string path)
        {
            return Path.GetFullPath(
                Path.IsPathRooted(path)
                    ? path
                    : Path.Combine(MountPoint, path));
        }
    }
    
}

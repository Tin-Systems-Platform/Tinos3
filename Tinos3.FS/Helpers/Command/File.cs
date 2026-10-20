
using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;

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
                    {
                        Console.Write(Path.GetFileName(dir));
                        Console.Write("  ");
                    }

                    if (hasColor)
                        Console.ForegroundColor = ConsoleColor.White;

                    foreach (string file in files)
                    {
                        Console.Write(Path.GetFileName(file));
                        Console.Write("  ");
                    }

                    Console.WriteLine();
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

        /// <summary>
        /// Resolves a path to its full path.
        /// </summary>
        public static string ResolvePath(string path)
        {
            return Path.GetFullPath(
                Path.IsPathRooted(path)
                    ? path
                    : Path.Combine(MountPoint, path));
        }

        public static void MakeDirectory(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                Console.WriteLine("No directory specified.");
                return;
            }
            string fullPath = ResolvePath(path);
            try
            {
                Directory.CreateDirectory(fullPath);
                Console.WriteLine($"Directory '{path}' created.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unable to create directory '{path}': {ex.Message}");
            }
        }

        public static void RemoveDirectory(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                Console.WriteLine("No directory specified.");
                return;
            }
            string fullPath = ResolvePath(path);
            try
            {
                Directory.Delete(fullPath, true);
                Console.WriteLine($"Directory '{path}' removed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unable to remove directory '{path}': {ex.Message}");
            }
        }

        public static void RemoveFile(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                Console.WriteLine("No file specified.");
                return;
            }
            string fullPath = ResolvePath(path);
            try
            {
                System.IO.File.Delete(fullPath);
                Console.WriteLine($"File '{path}' removed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unable to remove file '{path}': {ex.Message}");
            }
        }

        public static void MoveFile(string source, string destination)
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
                System.IO.File.Move(sourcePath, destinationPath);
                Console.WriteLine($"Moved '{source}' to '{destination}'.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Move failed: {ex.Message}");
            }
        }

        public static void MoveDirectory(string source, string destination)
        {
            if (string.IsNullOrWhiteSpace(source) ||
                string.IsNullOrWhiteSpace(destination))
            {
                Console.WriteLine("No source or destination specified.");
                return;
            }
            string sourcePath = ResolvePath(source);
            string destinationPath = ResolvePath(destination);
            if (!Directory.Exists(sourcePath))
            {
                Console.WriteLine($"Source directory '{source}' doesn't exist.");
                return;
            }
            try
            {
                Directory.Move(sourcePath, destinationPath);
                Console.WriteLine($"Moved '{source}' to '{destination}'.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Move failed: {ex.Message}");
            }
        }
        public static void RenameFile(string source, string newName)
        {
            if (string.IsNullOrWhiteSpace(source) ||
                string.IsNullOrWhiteSpace(newName))
            {
                Console.WriteLine("No source or new name specified.");
                return;
            }
            string sourcePath = ResolvePath(source);
            string destinationPath = Path.Combine(Path.GetDirectoryName(sourcePath) ?? "", newName);
            if (!System.IO.File.Exists(sourcePath))
            {
                Console.WriteLine($"Source file '{source}' doesn't exist.");
                return;
            }
            try
            {
                System.IO.File.Move(sourcePath, destinationPath);
                Console.WriteLine($"Renamed '{source}' to '{newName}'.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Rename failed: {ex.Message}");
            }
        }
        public static void RenameDirectory(string source, string newName)
        {
            if (string.IsNullOrWhiteSpace(source) ||
                string.IsNullOrWhiteSpace(newName))
            {
                Console.WriteLine("No source or new name specified.");
                return;
            }
            string sourcePath = ResolvePath(source);
            string destinationPath = Path.Combine(Path.GetDirectoryName(sourcePath) ?? "", newName);
            if (!Directory.Exists(sourcePath))
            {
                Console.WriteLine($"Source directory '{source}' doesn't exist.");
                return;
            }
            try
            {
                Directory.Move(sourcePath, destinationPath);
                Console.WriteLine($"Renamed '{source}' to '{newName}'.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Rename failed: {ex.Message}");
            }
        }

        public static void MakeFile(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                Console.WriteLine("No file specified.");
                return;
            }
            string fullPath = ResolvePath(path);
            try
            {
                using (System.IO.File.Create(fullPath)) { }
                Console.WriteLine($"File '{path}' created.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unable to create file '{path}': {ex.Message}");
            }
        }
    }

}

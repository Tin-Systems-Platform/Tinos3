using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using CommandMethods = Tinos3.FS.Helpers.Command;

namespace Tinos3.Abstractions.Applications.Nano
{
    public class NanoFileIO
    {
        private const string MountPoint = "/mnt";

        public static string ReadFile(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("No file specified.", nameof(path));

            string fullPath = ResolvePath(path);

            if (!File.Exists(fullPath))
            {
                string? directory = Path.GetDirectoryName(fullPath);

                if (string.IsNullOrEmpty(directory) ||
                    !Directory.Exists(directory))
                {
                    throw new DirectoryNotFoundException(
                        $"Directory for '{path}' does not exist.");
                }

                return string.Empty;
            }

            return File.ReadAllText(fullPath);
        }

        private static string ResolvePath(string path)
        {
            return Path.GetFullPath(
                Path.IsPathRooted(path)
                    ? path
                    : Path.Combine(MountPoint, path));
        }

        public static void WriteFile(string path, string content, bool overwrite = false)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                Console.WriteLine("No file specified.");
                return;
            }
            string fullPath = ResolvePath(path);

            string? directory = Path.GetDirectoryName(fullPath);

            if (!string.IsNullOrEmpty(directory))
                Directory.CreateDirectory(directory);

            if (System.IO.File.Exists(fullPath) && !overwrite)
            {
                Console.WriteLine($"File '{path}' already exists. Use overwrite option to replace it.");
                return;
            }
            try
            {
                System.IO.File.WriteAllText(fullPath, content);
                Console.WriteLine($"File '{path}' written successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unable to write file '{path}': {ex.Message}");
            }
        }
    }
}

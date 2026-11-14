using System;
using System.Collections.Generic;
using System.IO;

namespace Tinos3.Api.SysConfig
{
    public class GraphicalConfig
    {
        public bool EnableGraphicalMode { get; set; } = true;

        public void GetConfig()
        {
            try
            {

                if (!File.Exists("mnt/config/config.ini"))
                {
                    Console.WriteLine("Do you want to enable graphical mode? (Y/n):");

                    var inputOnFirstRun = Console.ReadLine();

                    EnableGraphicalMode =
                        inputOnFirstRun?.Trim().Equals(
                            "y", StringComparison.OrdinalIgnoreCase) == true;

                    SaveConfig();
                    return;
                }

                var lines = File.ReadAllLines("mnt/config/config.ini");

                foreach (var line in lines)
                {
                    var parts = line.Split('=', 2);

                    if (parts.Length != 2)
                        continue;

                    var key = parts[0].Trim();
                    var value = parts[1].Trim();

                    if (key.Equals("EnableGraphicalMode",
                        StringComparison.OrdinalIgnoreCase))
                    {
                        if (bool.TryParse(value, out var result))
                            EnableGraphicalMode = result;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[KERNEL]: Error reading config: {ex.Message}");
            }
        }

        public void SaveConfig()
        {
            var lines = new List<string>
            {
                $"EnableGraphicalMode={EnableGraphicalMode}"
            };

            try
            {
                File.WriteAllLines("mnt/config/config.ini", lines);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[KERNEL]: Error saving config: {ex.Message}");
            }
        }
    }
}
using Cosmos.Executable.Lua;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using Cosmos.Kernel.System.Network;
using Cosmos.Kernel.System.Timers;

namespace Tinos3.Abstractions.Networking
{
    public class Tinos3Network
    {
        public static void Initialize()
        {
            Console.WriteLine("[NET]: Initializing System Network");
            if (NetworkManager.DeviceCount > 0)
            {
                Console.WriteLine("[NET]: Found Device:  " + NetworkManager.Name);
                Console.WriteLine("[NET]: MAC:     " + NetworkManager.MacAddress?.ToString());
                Console.WriteLine("[NET]: Link up: " + NetworkManager.LinkUp);
                Console.WriteLine("[NET]: Ready:   " + NetworkManager.Ready);
            }
            else
            {
                Console.WriteLine("[NET]: No network device found for Tinos3 to use");
                return;
            }
            DhcpClient dhcpClient = new();

            if (dhcpClient.SendDiscoverPacket() != -1)
            {
                IPConfig? config = NetworkManager.Primary.IPConfig;
                if (config is not null)
                {
                    Console.WriteLine("[NET]: IP address tinos3 will be using is " + config.Address.ToString());
                    Console.WriteLine("       Subnet:     " + config.SubnetMask.ToString());
                    Console.WriteLine("       Gateway:    " + config.DefaultGateway.ToString());
                }
            }
            else
            {
                Console.WriteLine("[NET]: DHCP timed out");
                return;
            }
            Console.WriteLine("[NET]: Network setup was done succesfully");
        }

    }
}
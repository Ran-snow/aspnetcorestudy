using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Cookie
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

        public static List<string> GetActiveMacAddress(string separator = "-")
        {
            var macAddress = new List<string>();

            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();

            if (nics == null || nics.Length < 1)
            {
                Console.WriteLine("  No network interfaces found.");
                return macAddress;
            }

            foreach (NetworkInterface adapter in nics.Where(c =>
                c.NetworkInterfaceType != NetworkInterfaceType.Loopback && c.OperationalStatus == OperationalStatus.Up))
            {
                IPInterfaceProperties properties = adapter.GetIPProperties();

                var unicastAddresses = properties.UnicastAddresses;
                if (unicastAddresses.Any(temp => temp.Address.AddressFamily == AddressFamily.InterNetwork))
                {
                    var address = adapter.GetPhysicalAddress();
                    if (string.IsNullOrEmpty(separator))
                    {
                        macAddress.Add(address.ToString());
                    }
                    else
                    {
                        macAddress.Add(string.Join(separator, address.GetAddressBytes()));
                    }
                }
            }

            return macAddress;
        }
    }
}

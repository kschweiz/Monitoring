using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections.Specialized;
using System.Net.NetworkInformation;
using System.Diagnostics;

namespace DiskSpace
{
    class WLANUsage
    {
        public static int CheckWLANUsage(ProgressBar progressBar)
        {
            try
            {
                var nics = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();
                // Select desired NIC
                var nic = nics.Single(n => n.Name == "Ethernet");
                var reads = Enumerable.Empty<double>();
                var sw = new Stopwatch();
                var lastBr = nic.GetIPv4Statistics().BytesReceived;

                double kbsSum = 0;

                for (var i = 0; i < 100; i++)
                {
                    sw.Restart();
                    Thread.Sleep(100);
                    var elapsed = sw.Elapsed.TotalSeconds;
                    var br = nic.GetIPv4Statistics().BytesReceived;

                    var local = (br - lastBr) / elapsed;
                    lastBr = br;

                    // Keep last 20, ~2 seconds
                    reads = new[] { local }.Concat(reads).Take(20);
                    if (i % 10 == 0)
                    { // ~1 second
                        var bSec = reads.Sum() / reads.Count();
                        var kbs = (bSec * 8) / 1024;
                        kbsSum += kbs;
                    }

                    progressBar.StepForward();
                }
                return Convert.ToInt32(kbsSum / 10);
            }
            catch
            {
                return 0;
            }
        }

        public static void PrintTraffic(double traffic)
        {
            try
            {
                //momentan + zwei --> wenn mehr Up/Down dann wieder plus rechnen
                Console.CursorTop = Program.countDrives + Program.list.Count + Program.CURSORPRINTTRAFFIC;
                Console.CursorLeft = 0;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Traffic Average:");
                Console.Write(new string(' ', Console.WindowWidth));
                Console.CursorLeft = 0;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("I/F: ");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("Ethernet");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\tStatus: ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("OK");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\tTraffic: ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(traffic + " Kbps");
            }
            catch
            {
                Console.CursorTop = Program.countDrives + Program.list.Count + Program.CURSORPRINTTRAFFIC;
                Console.CursorLeft = 0;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Traffic Average:");
                Console.Write(new string(' ', Console.WindowWidth));
                Console.CursorLeft = 0;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("I/F: ");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("Ethernet");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\tStatus: ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("FAILED");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\tTraffic: ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(traffic + "---");

                Program.countBadUpDownLoad++;
            }
        }
    }
}

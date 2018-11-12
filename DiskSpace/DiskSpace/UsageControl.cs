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
    class UsageControl
    {
        //UsageControl
        public static PerformanceCounter cpuCounter = new PerformanceCounter();
        public static PerformanceCounter ramCounter = new PerformanceCounter();

        public static int[] usageAverage = new int[2];
        public static int[] usageCounter = new int[2];

        public static void CheckUsageControl()
        {
            cpuCounter.CategoryName = "Processor";
            cpuCounter.CounterName = "% Processor Time";
            cpuCounter.InstanceName = "_Total";

            ramCounter.CategoryName = "Memory";
            ramCounter.CounterName = "% Committed Bytes in Use";

            try
            {                
                int ram = Convert.ToInt32(ramCounter.NextValue());
                usageAverage[0] += ram;
                usageCounter[0]++;

                Console.CursorTop = 5;
                Console.CursorLeft = 0;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Performace Workload:");

                //RAM
                Console.CursorTop = 6;
                Console.CursorLeft = 0;
                Console.Write(new string(' ', Console.WindowWidth));
                Console.CursorLeft = 0;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("Name: ");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("RAM");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\tStatus: ");
                if (ram < 70)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("OK");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("\tUsage: ");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write($"{ram}%");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("BAD");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("\tUsage: ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write($"{ram}%");
                }
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\tAverage: ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write($"{usageAverage[0] / usageCounter[0]}%");
            }
            catch
            {
                //RAM
                Console.CursorTop = 6;
                Console.CursorLeft = 0;
                Console.Write(new string(' ', Console.WindowWidth));
                Console.CursorLeft = 0;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("Name: ");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("RAM");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\tStatus: ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("FAILED");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\tUsage: ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write($"---");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\tAverage: ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("---");
            }

            try
            {
                //CPU
                Console.CursorTop = 7;
                Console.CursorLeft = 0;
                int cpu = Convert.ToInt32(cpuCounter.NextValue());
                usageAverage[1] += cpu;
                usageCounter[1]++;

                Console.Write(new string(' ', Console.WindowWidth));
                Console.CursorLeft = 0;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("Name: ");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("CPU");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\tStatus: ");
                if (cpu < 70)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("OK");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("\tUsage: ");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write($"{cpu}%");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("BAD");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("\tUsage: ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write($"{cpu}%\n");
                }
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\tAverage: ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write($"{usageAverage[1] / usageCounter[1]}%");
            }
            catch
            {
                //CPU
                Console.CursorTop = 7;
                Console.CursorLeft = 0;
                Console.Write(new string(' ', Console.WindowWidth));
                Console.CursorLeft = 0;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("Name: ");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("CPU");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\tStatus: ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("FAILED");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\tUsage: ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write($"---");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\tAverage: ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("---");
            }
        }
    }
}

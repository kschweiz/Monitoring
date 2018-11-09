using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections.Specialized;
using System.Net.NetworkInformation;
using System.Diagnostics;
using System.IO;

namespace DiskSpace
{
    class DiskUsage
    {
        public static void CheckDiskSize()
        {
            Console.CursorTop = 9;
            Console.CursorLeft = 0;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Disk Space List:");
            DriveInfo[] Drives = DriveInfo.GetDrives(); // alle Laufwerke auslesen
            Program.countDrives = Drives.Count();
            Console.CursorTop = 10;
            Console.CursorLeft = 0;
            foreach (DriveInfo d in Drives)
            {
                Console.Write(new string(' ', Console.WindowWidth));
                Console.CursorLeft = 0;
                string name = d.Name;
                long totalSize;
                long freeSize;
                int prozent;
                try
                {
                    totalSize = (d.TotalSize / (1024 * 1024 * 1024));
                    freeSize = (d.TotalFreeSpace / (1024 * 1024 * 1024));
                    prozent = Convert.ToInt32(100.00 / totalSize * (totalSize - freeSize));
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write($"Name: ");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write(name);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("\tStatus: ");
                    if (prozent > 75)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("CARE");
                    }
                    else if (prozent > 95)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("FULL");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("OK");
                    }

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("\tFree: ");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write($"{freeSize} GB");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("\tFull: ");
                    if (prozent > 75)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write($"{prozent}%\n");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write($"{prozent}%\n");
                    }
                }
                catch
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write($"Name: ");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write(name);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("\tStatus: ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("FAILED");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("\tFree: ");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write($"---");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("\tFull: ");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write($"---\n");
                    Program.countFailDisk++;
                }

            }
        }
    }
}

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
    class UpAndDownload
    {
        public static void CheckUpAndDownload()
        {
            Console.CursorTop = Program.countDrives + Program.list.Count + 13;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Up and Download:");


            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                Console.Write(new string(' ', Console.WindowWidth));
                Console.CursorLeft = 0;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("I/F: ");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("Not available");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\tStatus: ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("FAILED");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\tUP: ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("---");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\tDOWN: ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("---\n");
                Program.countBadUpDownLoad++;
            }
            else
            {
                NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
                long teiler = Convert.ToInt64(1024 * 1024);

                foreach (NetworkInterface ni in interfaces)
                {
                    if (ni.Name == "WiFi" || ni.Name == "Ethernet")
                    {
                        try
                        {
                            long up = ni.GetIPv4Statistics().BytesSent / teiler;
                            long down = ni.GetIPv4Statistics().BytesReceived / teiler;
                            Console.Write(new string(' ', Console.WindowWidth));
                            Console.CursorLeft = 0;
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write("I/F: ");
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.Write(ni.Name);
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write("\tStatus: ");
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("OK");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write("\tUP: ");
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write(up);
                            Console.Write(" Mbps");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write("\tDOWN: ");
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write(down);
                            Console.Write(" Mbps\n");
                        }
                        catch
                        {
                            Console.Write(new string(' ', Console.WindowWidth));
                            Console.CursorLeft = 0;
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write("I/F: ");
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.Write(ni.Name);
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write("\tStatus: ");
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("FAILED");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write("\tUP: ");
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write("---");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write("\tDOWN: ");
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write("---\n");
                            Program.countBadUpDownLoad++;
                        }

                    }
                }

            }

        }
    }
}

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
    class Program
    {
        //Fail / Bad / Error Counters
        private static int countFailDisk = 0;
        private static int countBadPing = 0;
        private static int countBadUpDownLoad = 0;

        private static double traffic;

        static int countDrives;
        static NameValueCollection list = ConfigurationManager.AppSettings;
        static void Main(string[] args)
        {
            Console.WindowHeight = 30;
            Console.WindowWidth = 70;
            Console.Title = "Home Monitoring";
            Console.CursorVisible = false;
            Start();
            while (true)
            {
                Console.CursorTop = 1;
                Console.CursorLeft = 0;
                using (ProgressBar progressBar = new ProgressBar())
                {
                    progressBar.MaxSteps = 100;
                    traffic = CheckWLANUsage(progressBar);
                    //for(int i = 0; i<10; i++)
                    //{
                    //    Thread.Sleep(100);
                    //    progressBar.StepForward();
                    //}
                }
                Console.CursorLeft = 0;
                Console.Write("Getting Information...");


                Start();
                CheckDiskSize();
                PingTest();
                UpAndDownLoad();
                PrintTraffic(traffic);
                StatusLine();
            }



        }

        public static void StatusLine()
        {
            Console.CursorTop = 3;
            Console.CursorLeft = 0;
            Console.Write(new string(' ', Console.WindowWidth));
            Console.CursorTop = 3;
            Console.CursorLeft = 0;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Disk Fails: ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(countFailDisk);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("]");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("\tBad Pings: ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(countBadPing);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("]");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("\tBad Up/Down: ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(countBadUpDownLoad);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("]");
        }


        public static void Start()
        {

            Console.CursorTop = 0;
            Console.CursorLeft = 0;
            Console.Write(new string(' ', Console.WindowWidth));
            Console.CursorTop = 0;
            Console.CursorLeft = 0;
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("Home Monitoring   ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Last Update: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(DateTime.Now + "\n");
        }

        public static void CheckDiskSize()
        {
            Console.CursorTop = 5;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Disk Space List:");
            DriveInfo[] Drives = DriveInfo.GetDrives(); // alle Laufwerke auslesen
            countDrives = Drives.Count();
            Console.CursorTop = 6;
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
                    if(prozent > 75)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("CARE");
                    }
                    else if(prozent > 95)
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
                    if(prozent > 75)
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
                    countFailDisk++;
                }

            }
        }

        public static void PingTest()
        {
            Console.CursorTop = countDrives + 7;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Ping Test:");

            foreach(string s in list.AllKeys)
            {
                try
                {
                    Console.Write(new string(' ', Console.WindowWidth));
                    Console.CursorLeft = 0;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Ping ping = new Ping();
                    PingReply pr = ping.Send(list.Get(s));
                    if (pr.Status == IPStatus.Success)
                    {
                        Console.Write($"Ping: ");
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write(s);
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("\tStatus: ");
                        if (pr.RoundtripTime < 100)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("OK");
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("BAD");
                            countBadPing++;
                        }
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("\tTime: ");
                        if (pr.RoundtripTime < 100)
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write($"{pr.RoundtripTime} ms\n");
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write($"{pr.RoundtripTime} ms\n");
                        }
                    }
                    else
                    {
                        Console.Write($"Ping: ");
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write(s);
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("\tStatus: ");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("FAILED");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("\tTime: ");
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write($"---\n");
                        countBadPing++;
                    }
                }
                catch
                {
                    Console.Write($"Ping: ");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write(s);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("\tStatus: ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("FAILED");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("\tTime: ");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write($"---\n");
                    countBadPing++;
                }
                

            }
        }

        public static void UpAndDownLoad()
        {
            Console.CursorTop = countDrives + list.Count + 9;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Up and Download:");
            Console.Write(new string(' ', Console.WindowWidth));
            Console.CursorLeft = 0;

            if (!NetworkInterface.GetIsNetworkAvailable())
            {
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
                countBadUpDownLoad++;
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
                            countBadUpDownLoad++;
                        }

                    }
                }

            }

        }

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
                Console.CursorTop = countDrives + list.Count + 9 + 3;
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
                Console.CursorTop = countDrives + list.Count + 9 + 3;
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

                countBadUpDownLoad++;
            }

        }
    }
}

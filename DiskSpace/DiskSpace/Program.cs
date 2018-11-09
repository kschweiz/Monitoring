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
        public static int countFailDisk = 0;
        public static int countBadPing = 0;
        public static int countBadUpDownLoad = 0;

        public static double traffic;

        public static int countDrives;
        public static NameValueCollection list = ConfigurationManager.AppSettings;

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
                }
                Console.CursorLeft = 0;
                Console.Write("Getting Information...");

                Start();
                UsageControl.CheckUsageControl();
                DiskUsage.CheckDiskSize();
                PingTest.CheckPing();
                UpAndDownload.CheckUpAndDownload();
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
            Console.CursorLeft = 0;
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("Home Monitoring   ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Last Update: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(DateTime.Now + "\n");
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
                    if (true)
                    {

                    }
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
                Console.CursorTop = countDrives + list.Count + 13 + 3;
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
                Console.CursorTop = countDrives + list.Count + 13 + 3;
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

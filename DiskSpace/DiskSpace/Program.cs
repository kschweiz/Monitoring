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


        public const int CURSORPRINTTRAFFIC = 16;
        public const int CURSORCHECKUPDOWN = 13;
        public const int CURSORCHECKPING = 11;

        //Fail / Bad / Error Counters
        public static int countFailDisk = 0;
        public static int countBadPing = 0;
        public static int countBadUpDownLoad = 0;

        public static double traffic;

        public static int countDrives;
        public static NameValueCollection list = ConfigurationManager.AppSettings;
        public static long[] pingAverage = new long[list.Count];
        public static int[] pingCounter = new int[list.Count];
        

        static void Main(string[] args)
        {
            Console.WindowHeight = 32;
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
                    traffic = WLANUsage.CheckWLANUsage(progressBar);
                }
                Console.CursorLeft = 0;
                Console.Write("Getting Information...");

                Start();
                UsageControl.CheckUsageControl();
                DiskUsage.CheckDiskSize();
                PingTest.CheckPing();
                UpAndDownload.CheckUpAndDownload();
                WLANUsage.PrintTraffic(traffic);
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
    }
}

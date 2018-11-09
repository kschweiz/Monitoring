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
    class PingTest
    {
        public static void CheckPing()
        {
            Console.CursorTop = Program.countDrives + 11;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Ping Test:");

            foreach (string s in Program.list.AllKeys)
            {
                try
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Ping ping = new Ping();
                    PingReply pr = ping.Send(Program.list.Get(s));
                    Console.Write(new string(' ', Console.WindowWidth));
                    Console.CursorLeft = 0;
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
                            Program.countBadPing++;
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
                        Program.countBadPing++;
                    }
                }
                catch
                {
                    Console.Write(new string(' ', Console.WindowWidth));
                    Console.CursorLeft = 0;
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
                    Program.countBadPing++;
                }


            }
        }
    }
}

using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ServerSocket
{
    partial class Program
    {

        static void Main(String[] args)
        {
            Socket socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            IPAddress ip = IPAddress.Any;
            IPEndPoint point = new IPEndPoint(ip, 2333);
            socket.Bind(point);
            socket.Listen(10);
            Console.WriteLine("Listen Success");
            new Thread(Accept)
            {
                IsBackground = true
            }.Start(socket);
            while (true)
            {
                string line = Console.ReadLine().ToLower().Trim();
                if (line[0] == 'c')
                {
                    Console.Clear();
                }
            }

        }
    }
}

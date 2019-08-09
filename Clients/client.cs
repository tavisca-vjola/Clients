﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Finalclient
{
    class Program
    {
        static void Main(string[] args)
        {
            IPAddress ip = IPAddress.Parse("192.168.0.102");
            int port = 5000;
            TcpClient client = new TcpClient();
            client.Connect(ip, port);
            Console.WriteLine("Connected to Broadcast Server!!");
            Console.Write("Enter your Name: ");
            var userName = string.IsNullOrEmpty(Console.ReadLine()) ? "UnNamed User" : Console.ReadLine();
            NetworkStream ns = client.GetStream();
            Thread thread = new Thread(o => ReceiveData((TcpClient)o));
            thread.Start(client);
            string message;
            while (!string.IsNullOrEmpty((message = Console.ReadLine())) && message != "exit")
            {
                byte[] buffer = Encoding.ASCII.GetBytes($"{userName} : {message}");
                ns.Write(buffer, 0, buffer.Length);
            }

            Console.WriteLine("Disconnected from server!!");
            thread.Join();
            ns.Close();
            client.Close();
            Console.ReadKey();
        }

        static void ReceiveData(TcpClient client)
        {
            NetworkStream ns = client.GetStream();
            byte[] receivedBytes = new byte[1024];
            int byte_count;

            while ((byte_count = ns.Read(receivedBytes, 0, receivedBytes.Length)) > 0)
            {
                Console.WriteLine(Encoding.ASCII.GetString(receivedBytes, 0, byte_count));
            }
        }
    }
}
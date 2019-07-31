using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Clients
{
    class Server
    {
        static void Main(string[] args)
        {
            int port = 8080;
            string ipaddress = "172.16.5.154";
            Socket serverListner = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ipaddress),port);
            serverListner.Bind(ep);
            serverListner.Listen(100);
            Console.WriteLine("Server is Listening");
            Socket clientsocket = default(Socket);
            int counter = 0;
            Server server= new Server();
            while(true)
            {
                counter++;
                clientsocket = serverListner.Accept();
                Console.WriteLine(counter + "clients connected");
                var s = Task.Factory.StartNew(()=>server.Recieve(clientsocket));
                var p = Task.Factory.StartNew(()=>server.Send(clientsocket));
                //Thread thread = new Thread(new ThreadStart(()=>server.Recieve(clientsocket)));
              //  thread.Start(User);
            }
        } 
        public void  Send(Socket client)
        {
            while(true)
            {
                string MsgfromServer = Console.ReadLine();
                byte[] recieved = new byte[100];
                recieved = System.Text.Encoding.UTF8.GetBytes(MsgfromServer);
                client.Send(recieved, 0, MsgfromServer.Length, SocketFlags.None);
            }
        }
        public void Recieve(Socket client)
        {
            while (true)
            {
                byte[] msg = new byte[100];
                int size = client.Receive(msg);
                Console.WriteLine(System.Text.Encoding.UTF8.GetString(msg));
                
            }

        }
    }
}

using System;
using System.Net;
using System.Net.Sockets;


namespace mysocket
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server();

            server.Start("127.0.0.1",1234);

            while (true)
            {
                string str = Console.ReadLine();

                switch (str)
                {
                    case "quit":

                        return;
                }
            }
            
            
            //Socket listedid = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //IPAddress ipAdr = IPAddress.Parse("127.0.0.1");//server address

            //IPEndPoint ipEp = new IPEndPoint(ipAdr, 1234);

            //listedid.Bind(ipEp);

            //listedid.Listen(0);//0->listen to unlimited connections 

            //Console.WriteLine("Start server successfully");

            //while (true)
            //{
            //    Socket connfd = listedid.Accept();

            //    Console.WriteLine("Server accept");

            //    byte[] readbuff = new byte[1024];

            //    int count = connfd.Receive(readbuff);

            //    string str = System.Text.Encoding.Default.GetString(readbuff, 0, count);

            //    byte[] bytes = System.Text.Encoding.Default.GetBytes("Server output" + str);

            //    connfd.Send(bytes);
            //}


        }
    }
}

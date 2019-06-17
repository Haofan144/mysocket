using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace mysocket
{
    class Server
    {
        public Socket listedId;

        public Conn[] conns;

        public int maxConn = 50;

        //Access to the connection pool, return -1 if fail 
        public int NewIndex()
        {
            if (conns == null)
            {
                return -1;
            }
            for(int i = 0; i < conns.Length; i++)
            {
                if (conns[i] == null)
                {
                    conns[i] = new Conn();
                    return i;
                }
                else if (conns[i].isUsed==false)
                {
                    return i;
                }
            }
            return -1;
        }

        //Start the server 
        public void Start(string host, int port)
        {
            conns = new Conn[maxConn];

            for (int i = 0; i < maxConn; i++)
            {
                conns[i] = new Conn();
            }

            listedId = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPAddress ipAdr = IPAddress.Parse(host);

            IPEndPoint ipEp = new IPEndPoint(ipAdr, port);

            listedId.Bind(ipEp);

            listedId.Listen(maxConn);

            listedId.BeginAccept(AcceptCb, null);

            Console.WriteLine("Start server successfully");
        }

        private void AcceptCb(IAsyncResult ar)
        {
            try
            {
                Socket socket = listedId.EndAccept(ar);

                int index = NewIndex();

                if (index < 0)
                {
                    socket.Close();
                    Console.WriteLine("Warings: out of maxconn");

                }
                else
                {
                    Conn conn = conns[index];

                    conn.Init(socket);

                    string adr = conn.GetAddress();

                    Console.WriteLine("a connection from the client" + adr + "id is:" + index);

                    conn.socket.BeginReceive(conn.readBuff, conn.buffCount, conn.BuffRemain(), SocketFlags.None, ReceiveCb, null);



                }
                listedId.BeginAccept(AcceptCb, null);//loop
            }
            catch(Exception e)
            {
                Console.WriteLine("fail to call" + e.Message);
            }
        }


        private void ReceiveCb(IAsyncResult ar)
        {
            Conn conn = (Conn)ar.AsyncState;

            try
            {
                int count = conn.socket.EndReceive(ar);

                if (count <= 0)
                {
                    Console.WriteLine(conn.GetAddress() + "disconnect with the server");

                    conn.Close();

                    return;
                }
                string str = System.Text.Encoding.Default.GetString(conn.readBuff, 0, count);

                Console.WriteLine("received"+conn.GetAddress()+"data is"+str);

                str = conn.GetAddress() + ":" + str;

                byte[] bytes = System.Text.Encoding.Default.GetBytes(str);

                //boardcast msg
                for(int i = 0; i < conns.Length; i++)
                {
                    if (conns[i] == null)
                        continue;
                    if (!conns[i].isUsed)
                        continue;
                    Console.WriteLine("Boardcast the server msg to" + conns[i].GetAddress());

                    conns[i].socket.Send(bytes);
                }

                conn.socket.BeginReceive(conn.readBuff, conn.buffCount, conn.BuffRemain(), SocketFlags.None, ReceiveCb, conn);

            }
            catch(Exception e)
            {
                Console.WriteLine("receive" + conn.GetAddress() + "disconnect"+e.Message);

                conn.Close();
            }
        }

    }
}

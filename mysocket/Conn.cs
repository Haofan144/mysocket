using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace mysocket
{
    class Conn
    {
        public const int buff_size = 1024;

        public Socket socket;

        public bool isUsed = false;

        public byte[] readBuff = new byte[buff_size];

        public int buffCount = 0;

        public Conn()
        {
            readBuff = new byte[buff_size];
        }

        public void Init(Socket socket)

        {
            this.socket = socket;
            isUsed = true;
            buffCount = 0;
        }

        public int BuffRemain()
        {
            return buff_size - buffCount;
        }

        public string GetAddress()
        {
            if (!isUsed)
            
                return "unable to get";
            return socket.RemoteEndPoint.ToString();
        }

        public void Close()
        {
            if (!isUsed)
                return;
            Console.WriteLine(GetAddress()+"disconnect with the server");

            socket.Close();
            isUsed = false;
        }

    }
}

using System.Collections.Generic;
using ExpType;
using System.Net.Sockets;
using System;
using System.Text;

namespace ServerSocket.S2C
{
    public abstract class S2CProtocol
    {
        private PROTOCOLTYPE type;
        private Socket socket;
        private List<byte> byteList;

        public PROTOCOLTYPE Type
        {
            get
            {
                return type;
            }
        }

        public S2CProtocol(PROTOCOLTYPE type, Socket socket)
        {
            this.type = type;
            this.socket = socket;
            byteList = new List<byte> { (byte)type };
        }
        public void Add(byte b)
        {
            byteList.Add(b);
        }
        public void Add(Int16 i)
        {
            byte[] bytes = BitConverter.GetBytes(i);
            Add(bytes[0]);
            Add(bytes[1]);
        }
        public void Add(string s)
        {
            byte[] bytes = Encoding.Default.GetBytes(s);
            foreach(byte b in bytes)
            {
                Add(b);
            }
        }
        public abstract void Marshal();
        private void _SendData()
        {
            byte[] buffer = new byte[byteList.Count];
            for (int i = 0; i < byteList.Count; ++i)
            {
                buffer[i] = byteList[i];
            }
            int sendSize = socket.Send(buffer);
            
            System.Diagnostics.StackTrace ss = new System.Diagnostics.StackTrace(true);
            System.Reflection.MethodBase mb = ss.GetFrame(2).GetMethod();
            Console.WriteLine("mb.Name: " + mb.Name);
            Console.WriteLine("sendSize: " + sendSize);
        }
        public void SendData()
        {
            Marshal();
            _SendData();
        }
    }
}

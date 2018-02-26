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
        public PROTOCOLTYPE Type { get => type; }

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
            socket.Send(buffer);
        }
        public void SendData()
        {
            Marshal();
            _SendData();
        }
    }
}

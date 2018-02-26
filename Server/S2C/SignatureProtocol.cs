using ExpType;
using System;
using System.Net.Sockets;
using System.Text;

namespace ServerSocket.S2C
{
    public class SignatureProtocol : S2CProtocol
    {
        public string signature;
        public SignatureProtocol(Socket socket) : base(PROTOCOLTYPE.SIGNATURE, socket)
        {

        }
        public override void Marshal()
        {
            Int16 len = (Int16)signature.Length;
            Add(len);
            Add(signature);
            //var lens = BitConverter.GetBytes(len);
            //Add(lens[0]);//1
            //Add(lens[1]);//2
            //var sigs = Encoding.Default.GetBytes(signature);
            //foreach (var b in sigs)
            //    Add(b);
            //var buffers = Encoding.UTF8.GetBytes("Server Return Message");
            //socket.Send(buffers);
        }
    }
}

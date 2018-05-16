using ExpType;
using System.Net.Sockets;

namespace ServerSocket.S2C
{
    class RegisterRetProtocol : S2CProtocol
    {
        public RegisterRetProtocol(Socket socket) : base(PROTOCOLTYPE.REGISTER_RET, socket)
        {

        }
        private REGISTERTYPE res;

        public REGISTERTYPE Res { get => res; set => res = value; }

        public override void Marshal()
        {
            Add((byte)res);
        }
    }
}

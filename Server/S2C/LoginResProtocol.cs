using ExpType;
using System.Net.Sockets;

namespace ServerSocket.S2C
{
    public class LoginResProtocol : S2CProtocol
    {
        public LOGINRESTYPE res;
        public LoginResProtocol(Socket socket) : base(PROTOCOLTYPE.LOGINRES, socket)
        {

        }

        public override void Marshal()
        {
            Add((byte)res);
        }
    }
}

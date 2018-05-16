using ExpType;
using System.Net.Sockets;

namespace ServerSocket.S2C
{
    public class LoginResProtocol : S2CProtocol
    {
        public LoginResProtocol(Socket socket) : base(PROTOCOLTYPE.LOGINRES, socket)
        {

        }
        public LOGINRESTYPE res;
        public override void Marshal()
        {
            Add((byte)res);
        }
    }
}

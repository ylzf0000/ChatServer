using System.Net.Sockets;
using ExpType;
using ServerSocket.C2S;
using ServerSocket.S2C;

namespace ServerSocket
{
    partial class Program
    {
        static LOGINRESTYPE Verify(string username, string password)
        {
            return LOGINRESTYPE.SUCCESS;
        }

        static void OnUserPwdProtocol(C2SProtocol c2SProtocol, Socket socket)
        {
            UserPwdProtocol userPwdProtocol = c2SProtocol as UserPwdProtocol;
            if (userPwdProtocol == null)
                return;

            string user = userPwdProtocol.Username;
            string pwd = userPwdProtocol.Password;

            //校验用户名和密码
            LOGINRESTYPE ret = Verify(user, pwd);

            //发送登录结果
            LoginResProtocol loginResProtocol = new LoginResProtocol(socket)
            {
                res = ret
            };
            //loginResProtocol.SendData();

            //发送sig
            if (ret == LOGINRESTYPE.SUCCESS)
            {
                string sig;
                GenerateSignature(user, out sig);
                SignatureProtocol signatureProtocol = new SignatureProtocol(socket) { signature = sig };
                signatureProtocol.SendData();

            }
        }
    }
}
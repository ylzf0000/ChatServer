using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Net.Sockets;
using ExpType;
using ServerSocket.C2S;
using ServerSocket.S2C;
using System.Threading;

namespace ServerSocket
{
    partial class Program
    {
        static LOGINRESTYPE Verify(string username, string password)
        {
            DbProviderFactory dbProviderFactory = DbProviderFactories.GetFactory("System.Data.SQLite.EF6");
            using (var conn = dbProviderFactory.CreateConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["sqlite"].ConnectionString;
                conn.Open();
                DbCommand command = conn.CreateCommand();
                command.CommandText = string.Format(@"select count(user) from ( select * from account where user = ""{0}"" and pwd = ""{1}"" )", username, password);
                Console.WriteLine(command.CommandText);
                //command.CommandText = @"select * from account";

                command.CommandType = CommandType.Text;
                command.Prepare();
                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int size = reader.GetInt32(0);
                        if(size > 0)
                            return LOGINRESTYPE.SUCCESS;
                        else
                            return LOGINRESTYPE.ERROR;
                    }
                }
            }
            return LOGINRESTYPE.ERROR;
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
            loginResProtocol.SendData();
            Thread.Sleep(500);
            //发送sig
            if (ret == LOGINRESTYPE.SUCCESS)
            {
                GenerateSignature(user, out string sig);
                SignatureProtocol signatureProtocol = new SignatureProtocol(socket) { signature = sig };
                signatureProtocol.SendData();

            }
        }
    }
}
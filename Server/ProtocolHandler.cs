using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
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
                        Console.WriteLine(size);
                        return LOGINRESTYPE.SUCCESS;
                        //var user = reader["user"];
                        //var pwd = reader["pwd"];
                        //Console.WriteLine(user + " " + pwd);
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
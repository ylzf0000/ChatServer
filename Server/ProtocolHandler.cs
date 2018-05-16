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
                        if (size > 0)
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
                SignatureProtocol signatureProtocol = new SignatureProtocol(socket)
                {
                    signature = sig
                };
                signatureProtocol.SendData();

            }
        }

        static void OnRegisterProtocol(C2SProtocol c2SProtocol, Socket socket)
        {
            RegisterProtocol registerProtocol = c2SProtocol as RegisterProtocol;
            if (registerProtocol == null)
                return;
            DbProviderFactory dbProviderFactory = DbProviderFactories.GetFactory("System.Data.SQLite.EF6");
            using (var conn = dbProviderFactory.CreateConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["sqlite"].ConnectionString;
                conn.Open();
                DbCommand command = conn.CreateCommand();

                // 判断是否已有该用户名
                command.CommandText = string.Format(@"SELECT * FROM account where user = ""{0}"" ", registerProtocol.Username);
                command.CommandType = CommandType.Text;
                command.Prepare();
                bool isExist = false;
                using (IDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        isExist = true;
                    }
                }
                if (isExist)
                {
                    RegisterRetProtocol prc = new RegisterRetProtocol(socket)
                    {
                        Res = REGISTERTYPE.EXIST
                    };
                    prc.SendData();
                    return;
                }
                // 插入该用户名
                command.CommandText = string.Format(@"
                    INSERT INTO account (user, pwd) VALUES (""{0}"", ""{1}"")", registerProtocol.Username, registerProtocol.Password);
                Console.WriteLine(command.CommandText);
                //command.CommandText = @"select * from account";

                command.CommandType = CommandType.Text;
                command.Prepare();
                int col = command.ExecuteNonQuery();
                if (col > 0)
                {
                    RegisterRetProtocol prc = new RegisterRetProtocol(socket)
                    {
                        Res = REGISTERTYPE.SUCCESS
                    };
                    prc.SendData();
                }
            }
        }
    }
}
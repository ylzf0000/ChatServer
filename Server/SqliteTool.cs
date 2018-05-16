using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSocket
{
    class SqliteTool
    {
        static void Select(string tableName)
        {
            DbProviderFactory dbProviderFactory = DbProviderFactories.GetFactory("System.Data.SQLite.EF6");
            using (var conn = dbProviderFactory.CreateConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["sqlite"].ConnectionString;
                conn.Open();
                DbCommand command = conn.CreateCommand();
                command.CommandText = string.Format(@"select * from {0}", tableName);
                Console.WriteLine(command.CommandText);
                command.CommandType = CommandType.Text;
                command.Prepare();
                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int size = reader.GetInt32(0);
                    }
                }
            }
        }
    }
}

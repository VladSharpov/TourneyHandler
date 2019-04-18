using HandlerLibrary.DataAccess;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandlerLibrary
{
    public static class GlobalConfig
    {
        public static IDataConnection Connection { get; private set; }

        public static void InitializeConnections(DatabaseType type)
        {
            switch (type)
            {
                case DatabaseType.Sql:
                    var sql = new SqlConnector();
                    Connection = sql;
                    break;
                case DatabaseType.TextFile:
                    var text = new TextConnector();
                    Connection = text;
                    break;
                default:
                    break;
            }
        }

        public static string ConnectionString(string dbName)
        {
            return ConfigurationManager.ConnectionStrings[dbName].ConnectionString;
        }
    }
}

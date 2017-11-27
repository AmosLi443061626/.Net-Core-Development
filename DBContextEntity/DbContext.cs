using System;
using System.Collections.Generic;
using AntData.ORM.Data;
using AntData.ORM.DbEngine.Configuration;
using DBContextEntity.Mysql;
using System.Linq;
using System.Diagnostics;

namespace DBContextEntity
{
    public class DbContext
    {
        /// <summary>
        /// 连接字符串初始化
        /// </summary>
        public static DbContext Container = new DbContext();

    public static void Configure(string connectionString)
    {
            AntData.ORM.Common.Configuration.Linq.AllowMultipleQuery = true;
            AntData.ORM.Common.Configuration.Linq.IgnoreNullInsert = true;
            AntData.ORM.Common.Configuration.DBSettings = new DBSettings
            {
                DatabaseSettings = new List<DatabaseSettings>
                {
                    new DatabaseSettings
                    {
                        Name = "mysql",
                        Provider = "mySqlProvider",
                        ConnectionItemList = new List<ConnectionStringItem>
                        {
                            new ConnectionStringItem
                            {
                                Name = "mysql",
                                ConnectionString = connectionString
                            }
                        }
                    }
                }
            };
        }

        public DbContext<Entitys> Context
        {
            get
            {
                var db = new MysqlDbContext<Entitys>("mysql")
                {
                    IsEnableLogTrace = true,
                    OnLogTrace = OnCustomerTraceConnection
                };
                return db;
            }
        }

        private static void OnCustomerTraceConnection(CustomerTraceInfo customerTraceInfo)
        {
            try
            {
                string sql = customerTraceInfo.CustomerParams.Aggregate(customerTraceInfo.SqlText,
                        (current, item) => current.Replace(item.Key, item.Value.Value.ToString()));

                Debug.Write(sql + Environment.NewLine);
            }
            catch (Exception)
            {
                //ignore
            }
        }
    }
}

using System;
using System.Threading.Tasks;
using MySqlConnector;
namespace newApi;


    public static class DatabaseConnection
    {
        private static string Connectionstring= "Server=localhost;Port=3306;Database=newapi;Uid=user;Pwd=qaz";
        

        private static MysqlConnectionStringBuilder _instance = null;

        public static MysqlConnectionStringBuilder GetConnectiom()
        {
            if(_instance == null)
            {
                _instance = new MySqlConnectionStringBuilder{
                    ConnectionString;
                };
            }
        }
    }
   

using System;
using System.Threading.Tasks;
using MySqlConnector;
namespace newApi;
using System.Data;


public class MysqlConnect
{
    private static string Connectionstring = "Server=localhost;Port=3306;Database=newapi;Uid=user;Pwd=qaz";
    private MysqlConnect(){}

    private static MySqlConnection? _instance = null;

    public static MySqlConnection GetConnection()
    {
        if (_instance == null) // برای اینکه اتصال دیتابیس فقط یکبار ساخته شود و با هر بار استفاده دوباره ساخته نشود
        {
            _instance = new MySqlConnection(Connectionstring);
            _instance.Open();
        }
        return _instance;
    }

}
